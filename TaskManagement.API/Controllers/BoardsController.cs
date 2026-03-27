using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;

namespace TaskManagement.API.Controllers
{
    [Route("api/Boards")]
    [ApiController]
    public class BoardsController : ControllerBase
    {
        private readonly IBoardRepository boardRepository;
        private readonly IMapper mapper;
        protected ResponseDto responseDto;

        public BoardsController(IBoardRepository _boardRepository, IMapper _mapper)
        {
            boardRepository = _boardRepository;
            mapper = _mapper;
            responseDto = new();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var Boards = await boardRepository.GetAllAsync();
            responseDto.Result = mapper.Map<IEnumerable<BoardDto>>(Boards);
            return Ok(responseDto);
        }

        [HttpGet]
        [Route("GetByProject/{projectId:guid}")]
        public async Task<IActionResult> GetByProject(Guid projectId)
        {
            try
            {
                var BoardByProj = await boardRepository.GetAllByProjectAsync(projectId);
                //var test = mapper.Map<List<BoardDto>>(BoardByProj);
                responseDto.Result = mapper.Map<List<BoardDto>>(BoardByProj);
            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = ex.Message;
                return BadRequest(responseDto);
            }
            return Ok(responseDto);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var Board = await boardRepository.GetByIdAsync(id);
                if (Board == null) NotFound();
                responseDto.Result = mapper.Map<BoardDto>(Board);
            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = ex.Message;
                return BadRequest(responseDto);
            }
            return Ok(responseDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post(BoardDto boardDto)
        {
            try
            {
                var board = mapper.Map<Board>(boardDto);
                await boardRepository.AddAsync(board);
                await boardRepository.SaveChangesAsync();

                var generatedEntity = mapper.Map<BoardDto>(board);

                responseDto.Result = generatedEntity;
                return CreatedAtAction(nameof(Get), new { id = generatedEntity.Id }, responseDto);
            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = ex.Message;
                return BadRequest(responseDto);
            }
            //return Ok(responseDto);            
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, BoardDto boardDto)
        {
            try
            {
                var board = await boardRepository.GetByIdAsync(id);
                if (board == null) return NotFound($"Board with id {id} not found!");
                // this is the important part. Instead of creating a new object, it maps the incoming DTO fields onto the existing tracked entity, so EF Core knows what changed.
                mapper.Map(boardDto, board);

                await boardRepository.UpdateAsync(board);
                await boardRepository.SaveChangesAsync();

                responseDto.Result = mapper.Map<BoardDto>(board);
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
                var board = await boardRepository.GetByIdAsync(id);
                if (board == null) return NotFound($"Board with id {id} not found!");

                await boardRepository.DeleteAsync(board);
                await boardRepository.SaveChangesAsync();
                responseDto.Result = $"Board with id {id} is deleted successfully.";

                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.Message=ex.Message;
                return BadRequest(responseDto);
            }
        }

    }
}
