using FlowDesk.Api.Enums;

namespace FlowDesk.Api.DTOs.Task;

public class UpdateTaskDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public TaskPriority? Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public string? AssigneeEmployeeId { get; set; }
}