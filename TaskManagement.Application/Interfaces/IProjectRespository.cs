using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Interfaces
{
    public interface IProjectRespository
    {
        Task<IEnumerable<Project>> GetAllAsync(string UserId);
        Task<Project> GetByIdAsync(Guid Id);
        Task AddAsync(Project project);
        Task DeleteAsync(Project project);
        Task SaveChangesAsync();
    }
}
