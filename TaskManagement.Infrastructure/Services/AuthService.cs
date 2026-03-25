using Mango.Services.AuthAPI.Models.Dto;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagement.Application.DTOs.Auth;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Persistence;

namespace TaskManagement.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IJwtService jwtService;

        public AuthService(AppDbContext _db, UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager, IJwtService _jwtService)
        {
            db = _db;
            userManager = _userManager;
            roleManager = _roleManager;
            jwtService = _jwtService;
        }

        public Task<bool> AssignRole(string email, string roleName)
        {
            throw new NotImplementedException();
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.UserName);
            bool isValid = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            if (user == null || !isValid)
            {
                return new LoginResponseDto() { User = null, Token = "" };
            }
            var role = await userManager.GetRolesAsync(user);

            //if user found, generate JWT
            var token = jwtService.GenerateToken(user, role);

            UserDto userDto = new()
            {
                ID = user.Id,
                Email = user.Email,
                Name = user.FullName,
                PhoneNumber = user.PhoneNumber
            };

            LoginResponseDto loginResponseDto = new()
            {
                User = userDto,
                Token = token
            };

            return loginResponseDto;
        }

        public async Task<string> Regiser(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                FullName = registrationRequestDto.Name,
                UserName = registrationRequestDto.Email
            };
            try
            {
                var result = await userManager.CreateAsync(user, registrationRequestDto.Password);
                if (result.Succeeded)
                {
                    UserDto userDto = new()
                    {
                        ID = user.Id,
                        Email = user.Email,
                        Name = user.FullName
                    };
                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
