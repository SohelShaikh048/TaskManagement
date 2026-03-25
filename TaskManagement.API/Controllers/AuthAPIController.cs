using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.DTOs.Auth;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.API.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService authService;
        protected ResponseDto response;

        public AuthAPIController(IAuthService _authService)
        {
            authService = _authService;
            response = new ResponseDto();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            var errMsg = await authService.Regiser(registrationRequestDto);
            if (!string.IsNullOrEmpty(errMsg))
            {
                response.IsSuccess = false;
                response.Message = errMsg;
                return BadRequest(errMsg);
            }
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            var loginResponse = await authService.Login(loginRequestDto);
            if (loginResponse.User == null)
            {
                response.IsSuccess = false;
                response.Message = "Username or Password is incorrect!";
                return BadRequest(loginResponse);
            }
            response.Result = loginResponse;
            return Ok(response);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto model)
        {
            var assignRoleSuccessful = await authService.AssignRole(model.Email, model.Role);

            if (!assignRoleSuccessful)
            {
                response.IsSuccess = false;
                response.Message = "Error Encountered";
                return BadRequest(response);
            }
            return Ok(response);
        }

    }
}
