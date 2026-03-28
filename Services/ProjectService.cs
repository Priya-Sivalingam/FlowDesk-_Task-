using FlowDesk.Api.DTOs.Project;
using FlowDesk.Api.Entities;
using FlowDesk.Api.Repositories.Interfaces;
using FlowDesk.Api.Services.Interfaces;

namespace FlowDesk.Api.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUserRepository _userRepository;

    public ProjectService(IProjectRepository projectRepository, IUserRepository userRepository)
    {
        _projectRepository = projectRepository;
        _userRepository = userRepository;
    }

    public async Task<List<ProjectResponseDto>> GetAllProjectsAsync()
    {
        var projects = await _projectRepository.GetAllAsync();
        return projects.Select(MapToDto).ToList();
    }

    public async Task<ProjectResponseDto> GetProjectByIdAsync(Guid id)
    {
        var project = await _projectRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Project not found.");
        return MapToDto(project);
    }

    public async Task<ProjectResponseDto> CreateProjectAsync(CreateProjectDto dto, Guid createdByUserId)
    {
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            CreatedByUserId = createdByUserId,
            CreatedAt = DateTime.UtcNow
        };

        await _projectRepository.AddAsync(project);

        // Creator automatically becomes ProjectManager
        var member = new ProjectMember
        {
            Id = Guid.NewGuid(),
            ProjectId = project.Id,
            UserId = createdByUserId,
            Role = Enums.ProjectRole.ProjectManager
        };

        await _projectRepository.AddMemberAsync(member);
        await _projectRepository.SaveChangesAsync();

        var created = await _projectRepository.GetByIdAsync(project.Id);
        return MapToDto(created!);
    }

    public async Task<ProjectResponseDto> UpdateProjectAsync(Guid id, UpdateProjectDto dto)
    {
        var project = await _projectRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Project not found.");

        if (!string.IsNullOrWhiteSpace(dto.Name))
            project.Name = dto.Name;

        if (!string.IsNullOrWhiteSpace(dto.Description))
            project.Description = dto.Description;

        await _projectRepository.SaveChangesAsync();
        return MapToDto(project);
    }

    public async Task AddMemberAsync(Guid projectId, AddMemberDto dto)
    {
        var project = await _projectRepository.GetByIdAsync(projectId)
            ?? throw new KeyNotFoundException("Project not found.");

        var user = await _userRepository.GetByEmployeeIdAsync(dto.EmployeeId)
            ?? throw new KeyNotFoundException("User not found.");

        var alreadyMember = await _projectRepository.IsMemberAsync(projectId, user.Id);
        if (alreadyMember)
            throw new InvalidOperationException("User is already a member of this project.");

        var member = new ProjectMember
        {
            Id = Guid.NewGuid(),
            ProjectId = projectId,
            UserId = user.Id,
            Role = dto.Role
        };

        await _projectRepository.AddMemberAsync(member);
        await _projectRepository.SaveChangesAsync();
    }

    public async Task DeleteProjectAsync(Guid id)
    {
        var project = await _projectRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Project not found.");

        _projectRepository.Delete(project);
        await _projectRepository.SaveChangesAsync();
    }

    private static ProjectResponseDto MapToDto(Project p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description,
        CreatedByName = p.CreatedBy.Name,
        CreatedByEmployeeId = p.CreatedBy.EmployeeId,
        TotalTasks = p.Tasks.Count,
        TotalMembers = p.ProjectMembers.Count,
        CreatedAt = p.CreatedAt
    };
}