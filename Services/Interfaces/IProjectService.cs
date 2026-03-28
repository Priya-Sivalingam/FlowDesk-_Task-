using FlowDesk.Api.DTOs.Project;

namespace FlowDesk.Api.Services.Interfaces;

public interface IProjectService
{
    Task<List<ProjectResponseDto>> GetAllProjectsAsync();
    Task<ProjectResponseDto> GetProjectByIdAsync(Guid id);
    Task<ProjectResponseDto> CreateProjectAsync(CreateProjectDto dto, Guid createdByUserId);
    Task<ProjectResponseDto> UpdateProjectAsync(Guid id, UpdateProjectDto dto);
    Task AddMemberAsync(Guid projectId, AddMemberDto dto);
    Task DeleteProjectAsync(Guid id);
}
