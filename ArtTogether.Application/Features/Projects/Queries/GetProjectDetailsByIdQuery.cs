using ArtTogether.Application.DTOs;
using ArtTogether.Domain.Entities;
using ArtTogether.Domain.Interfaces;
using MediatR;

namespace ArtTogether.Application.Features.Projects.Queries;

public record GetProjectDetailsByIdQuery(Guid ProjectId, Guid UserId) : IRequest<ProjectDto?>;

public class GetProjectDetailsByIdQueryHandler(IProjectRepository repository)
    : IRequestHandler<GetProjectDetailsByIdQuery, ProjectDto?>
{
    private readonly IProjectRepository _repository = repository;

    public async Task<ProjectDto?> Handle(GetProjectDetailsByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetProjectByIdAsync(request.ProjectId);

        if (entity == null)
            throw new Exception("Proje bulunamadı.");

        var baseUrl = "https://arttogether.app";

        return new ProjectDetailsDto
        {
            ProjectId = entity.Id,
            ProjectName = entity.ProjectName,
            Width = entity.Width,
            Height = entity.Height,
            BackgroundColor = entity.BackgroundColor,
            Swatches = entity.Swatches,
            InvitationUrl = $"{baseUrl}/join/{entity.Id}",
            DeepLinkUrl = $"arttogether://project/{entity.Id}",
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            IsOwner = entity.CreatedUserId == request.UserId
        };
    }
}
