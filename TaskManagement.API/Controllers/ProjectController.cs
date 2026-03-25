using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;

namespace TaskManagement.API.Controllers
{
    [Authorize]
    [Route("api/project")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRespository projectRespository;
        protected ResponseDto response;

        public ProjectController(IProjectRespository _projectRespository)
        {
            projectRespository = _projectRespository;
            response = new();
        }

        private string GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var userId = GetUserId();
                var projects = await projectRespository.GetAllAsync(userId);
                response.Result = projects;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var project = await projectRespository.GetByIdAsync(id);
                if (project == null) return NotFound("Project with id " + id + " not found!");
                response.Result = project;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(ProjectDto dto)
        {
            try
            {
                var userId = GetUserId();
                Project project = new()
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    OwnerId = userId
                };

                await projectRespository.AddAsync(project);
                await projectRespository.SaveChangesAsync();
                response.Result = project;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var project = await projectRespository.GetByIdAsync(id);
                if (project == null) return NotFound();

                await projectRespository.DeleteAsync(project);
                await projectRespository.SaveChangesAsync();

                response.Result = project;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return BadRequest(response);
            }
            return NoContent();
        }

    }
}
