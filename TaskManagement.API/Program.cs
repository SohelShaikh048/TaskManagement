using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using TaskManagement.API;
using TaskManagement.Application.DTOs.Auth;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Persistence;
using TaskManagement.Infrastructure.Repositories;
using TaskManagement.Infrastructure.Seed;
using TaskManagement.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(d => d.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConn")));

#region Automapper
builder.Services.AddAutoMapper(typeof(AutomapperConfig));
#endregion Automapper

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));
var jwtOptions = builder.Configuration.GetSection("JwtOptions").Get<JwtOptions>();
var key = Encoding.UTF8.GetBytes(jwtOptions.Secret);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

builder.Services.AddAuthentication(d =>
{
    d.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    d.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(x =>
    {
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = jwtOptions.Audience,
            ValidIssuer = jwtOptions.Issuer,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

//ID for Services and Repositories
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProjectRespository, ProjectRespository>();
builder.Services.AddScoped<IBoardRepository, BoardRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

#region for accepting string values for enums in Postman
builder.Services.AddControllers()
    .AddJsonOptions(d =>
    {
        d.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
#endregion

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(d =>
    {
        d.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskManagement API v1");
        d.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

#region SeedingAdminData
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbSeeder.SeedAdminAsync(services);
}
#endregion SeedingAdminData

app.Run();
