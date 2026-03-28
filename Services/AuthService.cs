using Serilog;
using FlowDesk.Api.DTOs.Auth;
using FlowDesk.Api.Entities;
using FlowDesk.Api.Repositories.Interfaces;
using FlowDesk.Api.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace FlowDesk.Api.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _config;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUserRepository userRepository,
        IConfiguration config,
        ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _config = config;
        _logger = logger;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        _logger.LogInformation("Register attempt for email: {Email}", dto.Email);

        if (await _userRepository.EmailExistsAsync(dto.Email))
        {
            _logger.LogWarning("Register failed — email already exists: {Email}", dto.Email);
            throw new InvalidOperationException("Email already registered.");
        }

        var year = DateTime.UtcNow.Year;
        var count = await _userRepository.GetUserCountByYearAsync(year);
        var employeeId = $"FD-{year}-{(count + 1):D4}";

        var role = count == 0 ? Enums.SystemRole.Admin : Enums.SystemRole.Member;
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            EmployeeId = employeeId,
            Name = dto.Name,
            Email = dto.Email.ToLower(),
            PasswordHash = passwordHash,
            Role = role,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        _logger.LogInformation(
            "User registered successfully. EmployeeId: {EmployeeId}, Role: {Role}",
            employeeId, role);

        return new AuthResponseDto
        {
            EmployeeId = user.EmployeeId,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role.ToString(),
            Token = GenerateJwtToken(user)
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        _logger.LogInformation("Login attempt for EmployeeId: {EmployeeId}", dto.EmployeeId);

        var user = await _userRepository.GetByEmployeeIdAsync(dto.EmployeeId);

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            _logger.LogWarning("Login failed — invalid credentials for: {EmployeeId}", dto.EmployeeId);
            throw new UnauthorizedAccessException("Invalid employee ID or password.");
        }

        _logger.LogInformation(
            "Login successful. EmployeeId: {EmployeeId}, Role: {Role}",
            user.EmployeeId, user.Role);

        return new AuthResponseDto
        {
            EmployeeId = user.EmployeeId,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role.ToString(),
            Token = GenerateJwtToken(user)
        };
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim("uid", user.Id.ToString()),
            new Claim("employeeId", user.EmployeeId),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(
                double.Parse(_config["Jwt:ExpiryHours"]!)),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}