using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Interfaces
{
    public interface ITaskRepository : IGenericRepository<TaskItem>
    {
        Task<IEnumerable<TaskItem>> GetAllByBoardAsync(Guid Id);
    }
}
