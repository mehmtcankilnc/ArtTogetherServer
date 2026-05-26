using ArtTogether.Application.Interfaces;
using ArtTogether.Domain.Interfaces;
using MediatR;

namespace ArtTogether.Application.Features.Strokes.Commands;

public record UndoStrokeCommand(string ProjectId, Guid StrokeId) : IRequest;

public class UndoStrokeCommandHandler(IStrokeRepository repository, IDrawingNotifier notifier) : IRequestHandler<UndoStrokeCommand>
{
    private readonly IStrokeRepository _repository = repository;
    private readonly IDrawingNotifier _notifier = notifier;

    public async Task Handle(UndoStrokeCommand request, CancellationToken cancellationToken)
    {
        var stroke = await _repository.GetByStrokeIdAsync(request.StrokeId);

        if (stroke == null) throw new Exception("Stroke bulunamadı.");

        stroke.IsDeleted = true;

        await _repository.SaveAsync(stroke);
        await _notifier.BroadcastUndoStrokeAsync(request.ProjectId, request.StrokeId);
    }
}
