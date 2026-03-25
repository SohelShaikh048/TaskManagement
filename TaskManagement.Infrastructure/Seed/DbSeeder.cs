using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Seed
{
    public static class DbSeeder
    {
        public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            const string Email = "Admin@task.com";
            const string Pass = "India0091#";

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            var admin = await userManager.FindByEmailAsync(Email);

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = Email,
                    Email = Email,
                    NormalizedEmail = Email.ToUpper(),
                    EmailConfirmed = true,
                    FullName = "Test Admin"
                };

                await userManager.CreateAsync(admin, Pass);
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}
