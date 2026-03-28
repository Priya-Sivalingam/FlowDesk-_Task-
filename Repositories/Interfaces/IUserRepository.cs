using FlowDesk.Api.Entities;

namespace FlowDesk.Api.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByEmployeeIdAsync(string employeeId);
    Task<User?> GetByIdAsync(Guid id);
    Task<List<User>> GetAllAsync();
    Task<bool> EmailExistsAsync(string email);
    Task<int> GetUserCountByYearAsync(int year);
    Task AddAsync(User user);
    Task SaveChangesAsync();
    void Delete(User user);
}