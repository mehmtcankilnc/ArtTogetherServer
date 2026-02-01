using ArtTogether.Domain.Entities;
using ArtTogether.Domain.Interfaces;
using ArtTogether.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

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
            .Where(s => s.ProjectId == projectId)
            .OrderBy(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task ClearSessionAsync(Guid projectId)
    {
        await _context.Strokes
            .Where(s => s.ProjectId == projectId)
            .ExecuteDeleteAsync();
    }
}
