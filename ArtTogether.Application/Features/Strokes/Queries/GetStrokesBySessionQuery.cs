using ArtTogether.Application.DTOs;
using ArtTogether.Domain.Interfaces;
using MediatR;

namespace ArtTogether.Application.Features.Strokes.Queries;

public record GetStrokesBySessionQuery(Guid ProjectId) : IRequest<List<StrokeDto>>;

public class GetStrokesBySessionQueryHandler(IStrokeRepository repository)
    : IRequestHandler<GetStrokesBySessionQuery, List<StrokeDto>>
{
    private readonly IStrokeRepository _repository = repository;

    public async Task<List<StrokeDto>> Handle(GetStrokesBySessionQuery request, CancellationToken cancellationToken)
    {
        var strokes = await _repository.GetBySessionIdAsync(request.ProjectId);

        return strokes.Select(s => new StrokeDto
        {
            Color = s.Color,
            Width = s.Width,
            PathData = s.PathData,
        }).ToList();
    }
}

