using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs
{
    public class TaskItemDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public TaskItemStatus Status { get; set; }
        public int Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid BoardId { get; set; }
    }
}
