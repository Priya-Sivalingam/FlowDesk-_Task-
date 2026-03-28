using FlowDesk.Api.Enums;

namespace FlowDesk.Api.Entities;

public class ProjectMember
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public Guid UserId { get; set; }

    public ProjectRole Role { get; set; }

    public Project Project { get; set; } = null!;
    public User User { get; set; } = null!;
}