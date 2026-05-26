using ArtTogether.Domain.Entities;
using ArtTogether.Domain.Interfaces;
using ArtTogether.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace ArtTogether.Infrastructure.Repositories;

public class StrokeRepository(ApplicationDbContext context) : IStrokeRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task AddAsync(Stroke stroke)
    {
        await _context.Strokes.AddAsync(stroke);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Stroke>> GetBySessionIdAsync(Guid projectId)
    {
        return await _context.Strokes
            .AsNoTracking()
            .Where(s => s.ProjectId == projectId && s.IsDeleted == false)
            .OrderBy(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task ClearSessionAsync(Guid projectId)
    {
        await _context.Strokes
            .Where(s => s.ProjectId == projectId)
            .ExecuteDeleteAsync();
    }

    public async Task<Stroke?> GetByStrokeIdAsync(Guid strokeId)
    {
        return await _context.Strokes.FindAsync(strokeId);
    }

    public async Task SaveAsync(Stroke stroke)
    {
        _context.Update(stroke);
        await _context.SaveChangesAsync();
    }

    public async Task SoftDeleteStrokesByProjectIdAsync(Guid projectId)
    {
        await _context.Strokes
            .Where(s => s.ProjectId == projectId && s.IsDeleted == false)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsDeleted, true));
    }
}
