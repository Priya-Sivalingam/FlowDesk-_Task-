using System.Security.Claims;
using FlowDesk.Api.DTOs.Project;
using FlowDesk.Api.Models;
using FlowDesk.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlowDesk.Api.Controllers;

[ApiController]
[Route("api/projects")]
// [Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    // GET api/projects
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _projectService.GetAllProjectsAsync();
        return Ok(ApiResponse<List<ProjectResponseDto>>.Ok(result));
    }

    // GET api/projects/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var project = await _projectService.GetProjectByIdAsync(id);
        return Ok(ApiResponse<ProjectResponseDto>.Ok(project));
    }

    // POST api/projects
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateProjectDto dto)
    {
        var userId = Guid.Parse(User.FindFirstValue("uid")!);
        var project = await _projectService.CreateProjectAsync(dto, userId);
        return Ok(ApiResponse<ProjectResponseDto>.Ok(project, "Project created successfully."));
    }

    // PATCH api/projects/{id}
    [HttpPatch("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, UpdateProjectDto dto)
    {
        var updated = await _projectService.UpdateProjectAsync(id, dto);
        return Ok(ApiResponse<ProjectResponseDto>.Ok(updated, "Project updated successfully."));
    }

    // POST api/projects/{id}/members
    [HttpPost("{id:guid}/members")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddMember(Guid id, AddMemberDto dto)
    {
        await _projectService.AddMemberAsync(id, dto);
        return Ok(ApiResponse<string>.Ok("Member added successfully."));
    }

    // DELETE api/projects/{id}
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _projectService.DeleteProjectAsync(id);
        return Ok(ApiResponse<string>.Ok("Project deleted successfully."));
    }
}