using ArtTogether.Application.DTOs;
using ArtTogether.Domain.Interfaces;
using MediatR;

namespace ArtTogether.Application.Features.Projects.Queries;

public record GetProjectsByUserIdQuery(Guid UserId) : IRequest<List<ProjectDto>>;

public class GetProjectsByUserIdQueryHandler(IProjectRepository repository)
    : IRequestHandler<GetProjectsByUserIdQuery, List<ProjectDto>>
{
    private readonly IProjectRepository _repository = repository;

    public async Task<List<ProjectDto>> Handle(GetProjectsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var projects = await _repository.GetByUserIdAsync(request.UserId);

        var baseUrl = "https://arttogether.app";

        return projects.Select(p => new ProjectDto
        {
            ProjectId = p.Id,
            ProjectName = p.ProjectName,
            Width = p.Width,
            Height = p.Height,
            BackgroundColor = p.BackgroundColor,
            InvitationUrl = $"{baseUrl}/join/{p.Id}",
            DeepLinkUrl = $"arttogether://project/{p.Id}",
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt,
            IsOwner = p.CreatedUserId == request.UserId
        }).ToList();
    }
}
