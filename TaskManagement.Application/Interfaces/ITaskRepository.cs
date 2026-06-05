using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Interfaces
{
    public interface ITaskRepository : IGenericRepository<TaskItem>
    {
        Task<IEnumerable<TaskItem>> GetTasksByBoardAsync(Guid Id);

        Task<IEnumerable<TaskItem>> GetTasksByUserAsync(string userId);

        Task<IEnumerable<TaskItem>> GetUserTasksAsync(string userId);
    }
}
