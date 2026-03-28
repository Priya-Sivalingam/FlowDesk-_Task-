namespace FlowDesk.Api.DTOs.Project;

public class ProjectResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string CreatedByName { get; set; } = null!;
    public string CreatedByEmployeeId { get; set; } = null!;
    public int TotalTasks { get; set; }
    public int TotalMembers { get; set; }
    public DateTime CreatedAt { get; set; }
}