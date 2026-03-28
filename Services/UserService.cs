using FlowDesk.Api.DTOs.User;
using FlowDesk.Api.Repositories.Interfaces;
using FlowDesk.Api.Services.Interfaces;

namespace FlowDesk.Api.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<UserResponseDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(MapToDto).ToList();
    }

    public async Task<UserResponseDto> GetUserByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("User not found.");

        return MapToDto(user);
    }

    public async Task<UserResponseDto> UpdateUserAsync(Guid id, UpdateUserDto dto)
    {
        var user = await _userRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("User not found.");

        if (!string.IsNullOrWhiteSpace(dto.Name))
            user.Name = dto.Name;

        if (!string.IsNullOrWhiteSpace(dto.Email))
        {
            var emailTaken = await _userRepository.EmailExistsAsync(dto.Email);
            if (emailTaken && user.Email != dto.Email.ToLower())
                throw new InvalidOperationException("Email already in use.");

            user.Email = dto.Email.ToLower();
        }

        await _userRepository.SaveChangesAsync();
        return MapToDto(user);
    }

    public async Task DeleteUserAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("User not found.");

        _userRepository.Delete(user);
        await _userRepository.SaveChangesAsync();
    }

    private static UserResponseDto MapToDto(Entities.User user) => new()
    {
        Id = user.Id,
        EmployeeId = user.EmployeeId,
        Name = user.Name,
        Email = user.Email,
        Role = user.Role.ToString(),
        CreatedAt = user.CreatedAt
    };
}