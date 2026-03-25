using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;

namespace TaskManagement.API.Controllers
{
    [Route("api/TaskItem")]
    [ApiController]
    public class TaskItemController : ControllerBase
    {
        private readonly ITaskRepository repo;
        private readonly IMapper mapper;
        private ResponseDto responseDto;
        public TaskItemController(ITaskRepository _repo, IMapper _mapper)
        {
            repo = _repo;
            mapper = _mapper;
            responseDto = new();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var tasks = await repo.GetAllAsync();
            responseDto.Result = mapper.Map<IEnumerable<TaskItemDto>>(tasks);
            return Ok(responseDto);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var task = await repo.GetByIdAsync(id);
                if (task == null) return NotFound();
                responseDto.Result = mapper.Map<TaskItemDto>(task);
                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = ex.Message;
                return BadRequest(ex.Message);
            }            
        }

        [HttpGet]
        [Route("GetByBoard/{boardId:guid}")]
        public async Task<IActionResult> GetByBoard(Guid boardId)
        {
            try
            {
                var tasks = await repo.GetAllByBoardAsync(boardId);
                responseDto.Result = mapper.Map<IEnumerable<TaskItemDto>>(tasks);
                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = ex.Message;
                return BadRequest(responseDto);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(TaskItemDto taskItemDto)
        {
            try
            {
                var taskItem = mapper.Map<TaskItem>(taskItemDto);
                await repo.AddAsync(taskItem);
                await repo.SaveChangesAsync();

                var generatedEntity = mapper.Map<TaskItemDto>(taskItem);

                responseDto.Result = generatedEntity;
                return CreatedAtAction(nameof(Get), new { id = generatedEntity.Id }, responseDto);
            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.Message=ex.Message;
                return BadRequest(responseDto);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, TaskItemDto taskItemDto)
        {
            try
            {
                var taskItem = await repo.GetByIdAsync(id);
                if (taskItem == null) return NotFound();

                mapper.Map(taskItemDto, taskItem);

                await repo.UpdateAsync(taskItem);
                await repo.SaveChangesAsync();

                responseDto.Result = mapper.Map<TaskItemDto>(taskItem);
                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = ex.Message;
                return BadRequest(responseDto);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var taskItem = await repo.GetByIdAsync(id);
                if (taskItem == null) return NotFound();

                await repo.DeleteAsync(taskItem);
                await repo.SaveChangesAsync();
                responseDto.Result = $"Task Item with id {id} is deleted Successfully";
                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = ex.Message;
                return BadRequest(responseDto);
            }
        }

    }
}
