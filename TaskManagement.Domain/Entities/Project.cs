using TaskManagement.Domain.Common;

namespace TaskManagement.Domain.Entities
{
    public class Project : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;

        public string OwnerId { get; set; } = default!;
        public ApplicationUser Owner { get; set; } = default!;

        public ICollection<Board> Boards { get; set; } = new List<Board>();
    }
}
