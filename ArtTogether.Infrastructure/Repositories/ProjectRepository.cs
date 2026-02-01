using ArtTogether.Domain.Entities;
using ArtTogether.Domain.Interfaces;
using ArtTogether.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ArtTogether.Infrastructure.Repositories;

public class ProjectRepository(ApplicationDbContext context) : IProjectRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task CreateAsync(Project project, ProjectMember membership)
    {
        await _context.Projects.AddAsync(project);
        await _context.ProjectMembers.AddAsync(membership);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsMemberAsync(Guid projectId, Guid userId)
    {
        return await _context.ProjectMembers.AnyAsync(pm => 
            pm.ProjectId == projectId && pm.UserId == userId);
    }

    public async Task AddMemberAsync(ProjectMember membership)
    {
        await _context.ProjectMembers.AddAsync(membership);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Project>> GetByUserIdAsync(Guid userId)
    {
        return await _context.ProjectMembers
            .Where(pm => pm.UserId == userId)
            .Include(pm => pm.Project)
            .Select(pm => pm.Project)
            .OrderByDescending(p => p.UpdatedAt)
            .ToListAsync();
    }
}
