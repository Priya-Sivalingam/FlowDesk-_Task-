using FlowDesk.Api.Enums;

namespace FlowDesk.Api.DTOs.Task;

public class TaskResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string Priority { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTime DueDate { get; set; }
    public bool IsArchived { get; set; }
    public string ProjectName { get; set; } = null!;
    public string? AssigneeName { get; set; }
    public string? AssigneeEmployeeId { get; set; }
    public DateTime CreatedAt { get; set; }
}