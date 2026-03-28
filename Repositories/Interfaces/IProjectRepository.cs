using FlowDesk.Api.Entities;

namespace FlowDesk.Api.Repositories.Interfaces;

public interface IProjectRepository
{
    Task<List<Project>> GetAllAsync();
    Task<Project?> GetByIdAsync(Guid id);
    Task<bool> IsMemberAsync(Guid projectId, Guid userId);
    Task AddAsync(Project project);
    Task AddMemberAsync(ProjectMember member);
    void Delete(Project project);
    Task SaveChangesAsync();
}