using FlowDesk.Api.DTOs.Task;
using FlowDesk.Api.Enums;

namespace FlowDesk.Api.Services.Interfaces;

public interface ITaskService
{
    Task<List<TaskResponseDto>> GetProjectTasksAsync(Guid projectId, BoardTaskStatus? status, TaskPriority? priority, string? assigneeEmployeeId);
    Task<TaskResponseDto> GetTaskByIdAsync(Guid id);
    Task<TaskResponseDto> CreateTaskAsync(CreateTaskDto dto);
    Task<TaskResponseDto> UpdateTaskAsync(Guid id, UpdateTaskDto dto);
    Task<TaskResponseDto> UpdateTaskStatusAsync(Guid id, UpdateTaskStatusDto dto);
    Task ArchiveTaskAsync(Guid id);
    Task DeleteTaskAsync(Guid id);
}