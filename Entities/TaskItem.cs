using FlowDesk.Api.Enums;

namespace FlowDesk.Api.Entities;

public class TaskItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public BoardTaskStatus Status { get; set; } = BoardTaskStatus.ToDo; // updated
    public DateTime DueDate { get; set; }
    public bool IsArchived { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public Guid? AssignedToUserId { get; set; }
    public User? AssignedTo { get; set; }
}