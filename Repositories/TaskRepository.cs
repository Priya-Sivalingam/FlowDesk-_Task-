using FlowDesk.Api.Data;
using FlowDesk.Api.Entities;
using FlowDesk.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlowDesk.Api.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<TaskItem>> GetAllByProjectAsync(Guid projectId)
        => await _context.Tasks
            .Include(t => t.AssignedTo)
            .Include(t => t.Project)
            .Where(t => t.ProjectId == projectId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

    public async Task<TaskItem?> GetByIdAsync(Guid id)
        => await _context.Tasks
            .Include(t => t.AssignedTo)
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.Id == id);

    public async Task AddAsync(TaskItem task)
        => await _context.Tasks.AddAsync(task);

    public void Delete(TaskItem task)
        => _context.Tasks.Remove(task);

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}