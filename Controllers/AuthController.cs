using FlowDesk.Api.DTOs.Auth;
using FlowDesk.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using FlowDesk.Api.Models; 
namespace FlowDesk.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

   [HttpPost("register")]
public async Task<IActionResult> Register(RegisterDto dto)
{
    var result = await _authService.RegisterAsync(dto);
    return Ok(ApiResponse<AuthResponseDto>.Ok(result, "Registration successful."));
}

[HttpPost("login")]
public async Task<IActionResult> Login(LoginDto dto)
{
    var result = await _authService.LoginAsync(dto);
    return Ok(ApiResponse<AuthResponseDto>.Ok(result, "Login successful."));
}
}