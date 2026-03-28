using FlowDesk.Api.Data;
using FlowDesk.Api.Entities;
using FlowDesk.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlowDesk.Api.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Project>> GetAllAsync()
        => await _context.Projects
            .Include(p => p.CreatedBy)
            .Include(p => p.Tasks)
            .Include(p => p.ProjectMembers)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

    public async Task<Project?> GetByIdAsync(Guid id)
        => await _context.Projects
            .Include(p => p.CreatedBy)
            .Include(p => p.Tasks)
            .Include(p => p.ProjectMembers)
                .ThenInclude(pm => pm.User)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<bool> IsMemberAsync(Guid projectId, Guid userId)
        => await _context.ProjectMembers
            .AnyAsync(pm => pm.ProjectId == projectId && pm.UserId == userId);

    public async Task AddAsync(Project project)
        => await _context.Projects.AddAsync(project);

    public async Task AddMemberAsync(ProjectMember member)
        => await _context.ProjectMembers.AddAsync(member);

    public void Delete(Project project)
        => _context.Projects.Remove(project);

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}