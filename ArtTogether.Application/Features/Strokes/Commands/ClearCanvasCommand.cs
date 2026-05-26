using ArtTogether.Application.Interfaces;
using ArtTogether.Domain.Interfaces;
using MediatR;

namespace ArtTogether.Application.Features.Strokes.Commands;

public record ClearCanvasCommand(Guid ProjectId, Guid UserId) : IRequest;

public class ClearCanvasCommandHandler(IStrokeRepository repository) : IRequestHandler<ClearCanvasCommand>
{
    private readonly IStrokeRepository _repository = repository;

    public async Task Handle(ClearCanvasCommand request, CancellationToken cancellationToken)
    {
        await _repository.SoftDeleteStrokesByProjectIdAsync(request.ProjectId);
    }
}