using ArtTogether.Application.DTOs;
using ArtTogether.Application.Interfaces;
using ArtTogether.Domain.Entities;
using ArtTogether.Domain.Interfaces;
using MediatR;

namespace ArtTogether.Application.Features.Strokes.Commands;

public record SendStrokeCommand(Guid ProjectId, Guid UserId, StrokeDto StrokeData, int? BrushType = 0) : IRequest;

public class SendStrokeCommandHandler(IStrokeRepository repository, IDrawingNotifier notifier) : IRequestHandler<SendStrokeCommand>
{
    private readonly IStrokeRepository _repository = repository;
    private readonly IDrawingNotifier _notifier = notifier;

    public async Task Handle(SendStrokeCommand request, CancellationToken cancellationToken)
    {
        var stroke = new Stroke
        {
            ProjectId = request.ProjectId,
            UserId = request.UserId,
            Color = request.StrokeData.Color,
            Width = request.StrokeData.Width,
            PathData = request.StrokeData.PathData,
            CreatedAt = DateTime.UtcNow,
            Type = request.BrushType == 0 ? StrokeType.Brush : StrokeType.Eraser,
        };

        await _repository.AddAsync(stroke);
        await _notifier.BroadcastStrokeAsync(request.ProjectId.ToString(), request.UserId.ToString(), request.StrokeData, request.BrushType);
    }
}
