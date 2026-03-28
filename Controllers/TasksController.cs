using FlowDesk.Api.DTOs.Task;
using FlowDesk.Api.Enums;
using FlowDesk.Api.Models;
using FlowDesk.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlowDesk.Api.Controllers;

[ApiController]
[Route("api/tasks")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    // GET api/tasks/project/{projectId}?status=InProgress&priority=High
    [HttpGet("project/{projectId:guid}")]
    public async Task<IActionResult> GetProjectTasks(
        Guid projectId,
        [FromQuery] BoardTaskStatus? status,
        [FromQuery] TaskPriority? priority,
        [FromQuery] string? assigneeEmployeeId)
    {
        var tasks = await _taskService.GetProjectTasksAsync(
            projectId, status, priority, assigneeEmployeeId);
        return Ok(ApiResponse<List<TaskResponseDto>>.Ok(tasks));
    }

    // GET api/tasks/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);
        return Ok(ApiResponse<TaskResponseDto>.Ok(task));
    }

    // POST api/tasks
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateTaskDto dto)
    {
        var task = await _taskService.CreateTaskAsync(dto);
        return Ok(ApiResponse<TaskResponseDto>.Ok(task, "Task created successfully."));
    }

    // PATCH api/tasks/{id}
    [HttpPatch("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, UpdateTaskDto dto)
    {
        var task = await _taskService.UpdateTaskAsync(id, dto);
        return Ok(ApiResponse<TaskResponseDto>.Ok(task, "Task updated successfully."));
    }

    // PATCH api/tasks/{id}/status
    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, UpdateTaskStatusDto dto)
    {
        var task = await _taskService.UpdateTaskStatusAsync(id, dto);
        return Ok(ApiResponse<TaskResponseDto>.Ok(task, "Task status updated successfully."));
    }

    // PATCH api/tasks/{id}/archive
    [HttpPatch("{id:guid}/archive")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Archive(Guid id)
    {
        await _taskService.ArchiveTaskAsync(id);
        return Ok(ApiResponse<string>.Ok("Task archived successfully."));
    }

    // DELETE api/tasks/{id}
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _taskService.DeleteTaskAsync(id);
        return Ok(ApiResponse<string>.Ok("Task deleted successfully."));
    }
}