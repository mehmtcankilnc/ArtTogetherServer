using ArtTogether.Application.DTOs;

namespace ArtTogether.Application.Interfaces;

public interface IDrawingNotifier
{
    Task BroadcastStrokeAsync(string sessionId, string userId, StrokeDto stroke);
}
