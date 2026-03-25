using TaskManagement.Domain.Common;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Entities
{
    public class TaskItem : BaseEntity
    {
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public TaskItemStatus Status { get; set; }
        public int Priority { get; set; }
        public DateTime? DueDate { get; set; }

        public Guid BoardId { get; set; }
        public Board Board { get; set; } = default!;
    }
}
