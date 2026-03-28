using FlowDesk.Api.Data;
using FlowDesk.Api.Entities;
using FlowDesk.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlowDesk.Api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
        => await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<User?> GetByEmployeeIdAsync(string employeeId)
        => await _context.Users.FirstOrDefaultAsync(u => u.EmployeeId == employeeId);

    public async Task<bool> EmailExistsAsync(string email)
        => await _context.Users.AnyAsync(u => u.Email == email);

    public async Task<int> GetUserCountByYearAsync(int year)
        => await _context.Users
            .Where(u => u.CreatedAt.Year == year)
            .CountAsync();

    public async Task AddAsync(User user)
        => await _context.Users.AddAsync(user);

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();

    public async Task<User?> GetByIdAsync(Guid id)
    => await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

public async Task<List<User>> GetAllAsync()
    => await _context.Users.OrderBy(u => u.CreatedAt).ToListAsync();

    public void Delete(User user)
    => _context.Users.Remove(user);
}