using ArtTogether.Domain.Entities;

namespace ArtTogether.Domain.Interfaces;

public interface IProjectRepository
{
    Task CreateAsync(Project project, ProjectMember membership);
    Task<bool> IsMemberAsync(Guid projectId, Guid userId);
    Task AddMemberAsync(ProjectMember membership);
    Task<IEnumerable<Project>> GetByUserIdAsync(Guid userId);
    Task<Project?> GetProjectByIdAsync(Guid projectId);
    Task SaveChangesAsync();
}
