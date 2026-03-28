using FlowDesk.Api.DTOs.Task;
using FlowDesk.Api.Entities;
using FlowDesk.Api.Enums;
using FlowDesk.Api.Repositories.Interfaces;
using FlowDesk.Api.Services.Interfaces;

namespace FlowDesk.Api.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUserRepository _userRepository;
    private readonly IProjectRepository _projectRepository;

    public TaskService(
        ITaskRepository taskRepository,
        IUserRepository userRepository,
        IProjectRepository projectRepository)
    {
        _taskRepository = taskRepository;
        _userRepository = userRepository;
        _projectRepository = projectRepository;
    }

    public async Task<List<TaskResponseDto>> GetProjectTasksAsync(
        Guid projectId,
        BoardTaskStatus? status,
        TaskPriority? priority,
        string? assigneeEmployeeId)
    {
        var tasks = await _taskRepository.GetAllByProjectAsync(projectId);

        if (status.HasValue)
            tasks = tasks.Where(t => t.Status == status.Value).ToList();

        if (priority.HasValue)
            tasks = tasks.Where(t => t.Priority == priority.Value).ToList();

        if (!string.IsNullOrWhiteSpace(assigneeEmployeeId))
            tasks = tasks.Where(t =>
                t.AssignedTo != null &&
                t.AssignedTo.EmployeeId == assigneeEmployeeId).ToList();

        return tasks.Select(MapToDto).ToList();
    }

    public async Task<TaskResponseDto> GetTaskByIdAsync(Guid id)
    {
        var task = await _taskRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Task not found.");
        return MapToDto(task);
    }

    public async Task<TaskResponseDto> CreateTaskAsync(CreateTaskDto dto)
    {
        if (dto.DueDate < DateTime.UtcNow)
            throw new ArgumentException("Due date cannot be in the past.");

        var project = await _projectRepository.GetByIdAsync(dto.ProjectId)
            ?? throw new KeyNotFoundException("Project not found.");

        User? assignee = null;
        if (!string.IsNullOrWhiteSpace(dto.AssigneeEmployeeId))
        {
            assignee = await _userRepository.GetByEmployeeIdAsync(dto.AssigneeEmployeeId)
                ?? throw new KeyNotFoundException("Assignee not found.");
        }

        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            Priority = dto.Priority,
            Status = BoardTaskStatus.ToDo,
            DueDate = dto.DueDate,
            ProjectId = dto.ProjectId,
            AssignedToUserId = assignee?.Id,
            CreatedAt = DateTime.UtcNow
        };

        await _taskRepository.AddAsync(task);
        await _taskRepository.SaveChangesAsync();

        var created = await _taskRepository.GetByIdAsync(task.Id);
        return MapToDto(created!);
    }

    public async Task<TaskResponseDto> UpdateTaskAsync(Guid id, UpdateTaskDto dto)
    {
        var task = await _taskRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Task not found.");

        if (!string.IsNullOrWhiteSpace(dto.Title))
            task.Title = dto.Title;

        if (!string.IsNullOrWhiteSpace(dto.Description))
            task.Description = dto.Description;

        if (dto.Priority.HasValue)
            task.Priority = dto.Priority.Value;

        if (dto.DueDate.HasValue)
        {
            if (dto.DueDate.Value < DateTime.UtcNow)
                throw new ArgumentException("Due date cannot be in the past.");
            task.DueDate = dto.DueDate.Value;
        }

        if (!string.IsNullOrWhiteSpace(dto.AssigneeEmployeeId))
        {
            var assignee = await _userRepository.GetByEmployeeIdAsync(dto.AssigneeEmployeeId)
                ?? throw new KeyNotFoundException("Assignee not found.");
            task.AssignedToUserId = assignee.Id;
        }

        await _taskRepository.SaveChangesAsync();
        return MapToDto(task);
    }

    public async Task<TaskResponseDto> UpdateTaskStatusAsync(Guid id, UpdateTaskStatusDto dto)
    {
        var task = await _taskRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Task not found.");

        var valid = (task.Status, dto.Status) switch
        {
            (BoardTaskStatus.ToDo, BoardTaskStatus.InProgress) => true,
            (BoardTaskStatus.InProgress, BoardTaskStatus.Done) => true,
            (BoardTaskStatus.InProgress, BoardTaskStatus.Cancelled) => true,
            (BoardTaskStatus.ToDo, BoardTaskStatus.Cancelled) => true,
            _ => false
        };

        if (!valid)
            throw new InvalidOperationException(
                $"Cannot move task from {task.Status} to {dto.Status}.");

        task.Status = dto.Status;
        await _taskRepository.SaveChangesAsync();
        return MapToDto(task);
    }

    public async Task ArchiveTaskAsync(Guid id)
    {
        var task = await _taskRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Task not found.");

        if (task.Status != BoardTaskStatus.Done && task.Status != BoardTaskStatus.Cancelled)
            throw new InvalidOperationException(
                "Only completed or cancelled tasks can be archived.");

        task.IsArchived = true;
        await _taskRepository.SaveChangesAsync();
    }

    public async Task DeleteTaskAsync(Guid id)
    {
        var task = await _taskRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Task not found.");

        _taskRepository.Delete(task);
        await _taskRepository.SaveChangesAsync();
    }

    private static TaskResponseDto MapToDto(TaskItem t) => new()
    {
        Id = t.Id,
        Title = t.Title,
        Description = t.Description,
        Priority = t.Priority.ToString(),
        Status = t.Status.ToString(),
        DueDate = t.DueDate,
        IsArchived = t.IsArchived,
        ProjectName = t.Project.Name,
        AssigneeName = t.AssignedTo?.Name,
        AssigneeEmployeeId = t.AssignedTo?.EmployeeId,
        CreatedAt = t.CreatedAt
    };
}