using ArtTogether.Domain.Entities;

namespace ArtTogether.Domain.Interfaces;

public interface IStrokeRepository
{
    Task AddAsync(Stroke stroke);
    Task<IEnumerable<Stroke>> GetBySessionIdAsync(string sessionId);
    Task ClearSessionAsync(string sessionId);
}
