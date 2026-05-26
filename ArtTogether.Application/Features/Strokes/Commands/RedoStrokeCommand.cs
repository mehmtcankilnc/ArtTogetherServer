using ArtTogether.Application.DTOs;
using ArtTogether.Application.Interfaces;
using ArtTogether.Domain.Entities;
using ArtTogether.Domain.Interfaces;
using MediatR;

namespace ArtTogether.Application.Features.Strokes.Commands;

public record RedoStrokeCommand(string ProjectId, Guid StrokeId) : IRequest<StrokeDto>;

public class ReddoStrokeCommandHandler(IStrokeRepository repository, IDrawingNotifier notifier) : IRequestHandler<RedoStrokeCommand, StrokeDto>
{
    private readonly IStrokeRepository _repository = repository;
    private readonly IDrawingNotifier _notifier = notifier;

    public async Task<StrokeDto> Handle(RedoStrokeCommand request, CancellationToken cancellationToken)
    {
        var stroke = await _repository.GetByStrokeIdAsync(request.StrokeId);

        if (stroke == null) throw new Exception("Stroke bulunamadı.");

        stroke.IsDeleted = false;

        var strokeDto = new StrokeDto 
        {
            Id = stroke.Id, 
            Color = stroke.Color, 
            Width = stroke.Width, 
            PathData = stroke.PathData, 
            IsEraser = stroke.Type == StrokeType.Eraser 
        };

        await _repository.SaveAsync(stroke);
        await _notifier.BroadcastRedoStrokeAsync(request.ProjectId, strokeDto);

        return strokeDto;
    }
}
