using FlowDesk.Api.Entities;
using FlowDesk.Api.Enums;

namespace FlowDesk.Api.Repositories.Interfaces;

public interface ITaskRepository
{
    Task<List<TaskItem>> GetAllByProjectAsync(Guid projectId);
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task AddAsync(TaskItem task);
    void Delete(TaskItem task);
    Task SaveChangesAsync();
}