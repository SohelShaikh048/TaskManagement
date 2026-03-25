using Mango.Services.AuthAPI.Models.Dto;
using TaskManagement.Application.DTOs.Auth;

namespace TaskManagement.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> Regiser(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<bool> AssignRole(string email, string roleName);
    }
}
