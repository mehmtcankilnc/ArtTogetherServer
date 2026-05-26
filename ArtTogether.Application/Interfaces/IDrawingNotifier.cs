using ArtTogether.Application.DTOs;

namespace ArtTogether.Application.Interfaces;

public interface IDrawingNotifier
{
    Task BroadcastStrokeAsync(string sessionId, string userId, StrokeDto stroke, int? brushType);
    Task BroadcastUndoStrokeAsync(string projectId, Guid strokeId);
    Task BroadcastRedoStrokeAsync(string projectId, StrokeDto stroke);
}
