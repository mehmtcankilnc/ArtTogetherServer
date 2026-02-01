using ArtTogether.Application.DTOs;
using ArtTogether.Application.Interfaces;
using ArtTogether.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ArtTogether.Infrastructure.Services;

public class SignalRDrawingNotifier(IHubContext<DrawingHub, IDrawingHubClient> hubContext) : IDrawingNotifier
{
    private readonly IHubContext<DrawingHub, IDrawingHubClient> _hubContext = hubContext;

    public async Task BroadcastStrokeAsync(string projectId, string userId, StrokeDto stroke, int? brushType)
    {
        await _hubContext.Clients.Group(projectId).ReceiveStroke(userId, stroke, brushType);
    }
}
