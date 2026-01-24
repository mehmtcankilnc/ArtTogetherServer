using ArtTogether.Application.DTOs;
using ArtTogether.Application.Features.Strokes.Commands;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace ArtTogether.Infrastructure.Hubs;

public interface IDrawingHubClient
{
    Task ReceiveStroke(string userId, StrokeDto stroke);
    Task UserJoined(string userId);
    Task UserLeft(string userId);
}

public class DrawingHub(IMediator mediator) : Hub<IDrawingHubClient>
{
    private readonly IMediator _mediator = mediator;

    public async Task JoinRoom(string roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        await Clients.Group(roomId).UserJoined(Context.ConnectionId);
    }

    public async Task LeaveRoom(string roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        await Clients.Group(roomId).UserLeft(Context.ConnectionId);
    }

    public async Task SendStroke(string roomId, StrokeDto stroke)
    {
        var command = new SendStrokeCommand(roomId, Context.ConnectionId, stroke);

        await _mediator.Send(command);
    }
}