namespace FlowDesk.Api.DTOs.User;

public class UserResponseDto
{
    public Guid Id { get; set; }
    public string EmployeeId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}