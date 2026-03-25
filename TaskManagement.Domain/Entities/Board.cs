using System;
using System.Collections.Generic;
using System.Text;
using TaskManagement.Domain.Common;

namespace TaskManagement.Domain.Entities
{
    public class Board : BaseEntity
    {
        public string Name { get; set; } = default!;

        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = default!;

        public ICollection<TaskItem> TaskItems { get; set; } = new List<TaskItem>();
    }
}
