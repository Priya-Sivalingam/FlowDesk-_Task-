using FlowDesk.Api.Enums;

namespace FlowDesk.Api.DTOs.Project;

public class AddMemberDto
{
    public string EmployeeId { get; set; } = null!;   // who to add
    public ProjectRole Role { get; set; }              // their role
}