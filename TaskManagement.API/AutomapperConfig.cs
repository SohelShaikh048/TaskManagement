using AutoMapper;
using TaskManagement.Application.DTOs;
using TaskManagement.Domain.Entities;

namespace TaskManagement.API
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<Project, ProjectDto>().ReverseMap()
                .ForMember(dest => dest.Id, d => d.Ignore())
                .ForMember(dest => dest.CreatedAt, d => d.Ignore());

            CreateMap<Board, BoardDto>().ReverseMap()
                .ForMember(dest => dest.Id, d => d.Ignore())
                .ForMember(dest => dest.CreatedAt, d => d.Ignore());

            CreateMap<TaskItem, TaskItemDto>().ReverseMap()
                .ForMember(dest => dest.Id, d => d.Ignore())
                .ForMember(dest => dest.CreatedAt, d => d.Ignore());
        }
    }
}
