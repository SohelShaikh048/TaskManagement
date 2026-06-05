using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

namespace TaskManagement.API.Controllers
{
    [Authorize]
    [Route("api/project")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRespository projectRespository;
        private readonly IProjectMemberRepository projectMemberRepo;
        private readonly IMapper mapper;
        protected ResponseDto response;

        public ProjectController(IProjectRespository _projectRespository, IProjectMemberRepository _projectMemberRepo, IMapper _mapper)
        {
            projectRespository = _projectRespository;
            this.projectMemberRepo = _projectMemberRepo;
            mapper = _mapper;
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
                var projects = await projectRespository.GetAllAsync();
                response.Result = mapper.Map<IEnumerable<ProjectDto>>(projects);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("GetByUser")]
        public async Task<IActionResult> GetByUser()
        {
            try
            {
                var userId = GetUserId();
                var projects = await projectRespository.GetProjectsByUserAsync(userId);
                response.Result = mapper.Map<IEnumerable<ProjectDto>>(projects);
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
                response.Result = mapper.Map<ProjectDto>(project);
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
                dto.OwnerId = GetUserId();
                var project = mapper.Map<Project>(dto);

                await projectRespository.AddAsync(project);

                #region Adding creator as Admin in ProjectMember
                ProjectMember member = new()
                {
                    ProjectId = project.Id,
                    UserId = dto.OwnerId,
                    Role = MemberRoleEnum.Admin
                };
                await projectMemberRepo.AddAsync(member);
                #endregion

                await projectRespository.SaveChangesAsync();
                var createdProject = mapper.Map<ProjectDto>(project);
                response.Result = createdProject;
                return CreatedAtAction(nameof(Get), new { id = project.Id }, response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return BadRequest(response);
            }
            //return Ok(response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(Guid id, ProjectDto dto)
        {
            try
            {
                var project = await projectRespository.GetByIdAsync(id);
                if (project == null) return NotFound();

                // this is the important part. Instead of creating a new object, it maps the incoming DTO fields onto the existing tracked entity, so EF Core knows what changed.
                mapper.Map(dto, project);

                await projectRespository.UpdateAsync(project);
                await projectRespository.SaveChangesAsync();

                response.Result = mapper.Map<ProjectDto>(project);
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

                response.Result = mapper.Map<ProjectDto>(project);
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
