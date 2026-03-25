namespace TaskManagement.Application.DTOs
{
    public record ProjectDto
    {
        public Guid Id {  get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;

        public string? OwnerId { get; set; } = default!;
    }
}
