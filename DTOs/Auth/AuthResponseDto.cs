namespace FlowDesk.Api.DTOs.Auth;

public class AuthResponseDto
{
    public string EmployeeId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string Token { get; set; } = null!;    // JWT token
}