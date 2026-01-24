using ArtTogether.Application.DTOs;
using ArtTogether.Application.Interfaces;
using ArtTogether.Domain.Entities;
using ArtTogether.Domain.Interfaces;
using MediatR;

namespace ArtTogether.Application.Features.Strokes.Commands;

public record SendStrokeCommand(string SessionId, string UserId, StrokeDto StrokeData) : IRequest;

public class SendStrokeCommandHandler(IStrokeRepository repository, IDrawingNotifier notifier) : IRequestHandler<SendStrokeCommand>
{
    private readonly IStrokeRepository _repository = repository;
    private readonly IDrawingNotifier _notifier = notifier;

    public async Task Handle(SendStrokeCommand request, CancellationToken cancellationToken)
    {
        var stroke = new Stroke
        {
            SessionId = request.SessionId,
            UserId = request.UserId,
            Color = request.StrokeData.Color,
            Width = request.StrokeData.Width,
            PathData = request.StrokeData.PathData
        };

        await _repository.AddAsync(stroke);
        await _notifier.BroadcastStrokeAsync(request.SessionId, request.UserId, request.StrokeData);
    }
}
