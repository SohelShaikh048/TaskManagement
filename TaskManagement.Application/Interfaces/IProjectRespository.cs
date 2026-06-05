using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Interfaces
{
    public interface IProjectRespository : IGenericRepository<Project>
    {
        Task<IEnumerable<Project>> GetProjectsByUserAsync(string UserId);
    }
}
