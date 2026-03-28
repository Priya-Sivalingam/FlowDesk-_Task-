namespace FlowDesk.Api.Entities;

public class Project
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }

    public Guid CreatedByUserId { get; set; }
    public User CreatedBy { get; set; } = null!;

    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    public ICollection<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();
}