namespace FlowDesk.Api.DTOs.Auth;

public class LoginDto
{
    public string EmployeeId { get; set; } = null!;   // login with employee id
    public string Password { get; set; } = null!;
}