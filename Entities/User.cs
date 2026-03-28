namespace FlowDesk.Api.Entities;
using FlowDesk.Api.Enums;

public class User
{
    public Guid Id { get; set; }
    public string EmployeeId { get; set; } = null!;   // ← FD-2025-0001
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public SystemRole Role { get; set; } = SystemRole.Member;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Project> CreatedProjects { get; set; } = new List<Project>();
    public ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();
    public ICollection<ProjectMember> ProjectMemberships { get; set; } = new List<ProjectMember>();
}