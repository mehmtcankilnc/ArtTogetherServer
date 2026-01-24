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

    public async Task<IEnumerable<Stroke>> GetBySessionIdAsync(string sessionId)
    {
        return await _context.Strokes
            .AsNoTracking()
            .Where(s => s.SessionId == sessionId)
            .OrderBy(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task ClearSessionAsync(string sessionId)
    {
        await _context.Strokes
            .Where(s => s.SessionId == sessionId)
            .ExecuteDeleteAsync();
    }
}
