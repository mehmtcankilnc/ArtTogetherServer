using ArtTogether.Application.DTOs;
using ArtTogether.Application.Features.Strokes.Commands;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ArtTogether.Infrastructure.Hubs;

public interface IDrawingHubClient
{
    Task ReceiveStroke(string userId, StrokeDto stroke, int? brushType);
    Task UserJoined(string userId);
    Task UserLeft(string userId);
}

public class DrawingHub(IMediator mediator) : Hub<IDrawingHubClient>
{
    private readonly IMediator _mediator = mediator;

    public async Task JoinRoom(string projectId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, projectId);
        
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        await Clients.OthersInGroup(projectId).UserJoined(userId ?? "Anonymous");
    }

    public async Task LeaveRoom(string roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        await Clients.Group(roomId).UserLeft(Context.ConnectionId);
    }

    public async Task SendStroke(string projectId, StrokeDto stroke)
    {
        var projectGuid = Guid.Parse(projectId);

        var userIdValue = Context.User?.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value
                         ?? Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdValue))
        {
            throw new HubException("Kullanıcı kimliği bulunamadı. Lütfen giriş yaptığınızdan emin olun.");
        }

        var userGuid = Guid.Parse(userIdValue);
        var command = new SendStrokeCommand(projectGuid, userGuid, stroke);

        await _mediator.Send(command);

        await Clients.OthersInGroup(projectId).ReceiveStroke(userIdValue, stroke, 1);
    }
}