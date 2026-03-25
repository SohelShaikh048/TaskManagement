using AutoMapper;
using TaskManagement.Application.DTOs;
using TaskManagement.Domain.Entities;

namespace TaskManagement.API
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<Project, ProjectDto>().ReverseMap();
            CreateMap<Board, BoardDto>().ReverseMap();
            CreateMap<TaskItem, TaskItemDto>().ReverseMap();
        }
    }
}
