using ArtTogether.Domain.Entities;
using ArtTogether.Domain.Interfaces;
using MediatR;

namespace ArtTogether.Application.Features.Projects.Commands;

public record CreateProjectCommand(string ProjectName, Guid OwnerId) : IRequest<Guid>;

public class CreateProjectCommandHandler(IProjectRepository repository) : IRequestHandler<CreateProjectCommand, Guid>
{
    private readonly IProjectRepository _repository = repository;

    public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = new Project
        {
            Id = Guid.NewGuid(),
            ProjectName = request.ProjectName,
            CreatedUserId = request.OwnerId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var membership = new ProjectMember
        {
            ProjectId = project.Id,
            UserId = request.OwnerId,
            Role = ProjectRole.Owner
        };

        await _repository.CreateAsync(project, membership);

        return project.Id;
    }
}
