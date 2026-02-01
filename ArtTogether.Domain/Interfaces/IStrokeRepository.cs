using ArtTogether.Domain.Entities;

namespace ArtTogether.Domain.Interfaces;

public interface IStrokeRepository
{
    Task AddAsync(Stroke stroke);
    Task<IEnumerable<Stroke>> GetBySessionIdAsync(Guid projectId);
    Task ClearSessionAsync(Guid projectId);
}
