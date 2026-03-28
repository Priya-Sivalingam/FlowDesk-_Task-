using FlowDesk.Api.Enums;

namespace FlowDesk.Api.DTOs.Task;

public class CreateTaskDto
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public DateTime DueDate { get; set; }
    public Guid ProjectId { get; set; }
    public string? AssigneeEmployeeId { get; set; }   // optional, assign to someone
}