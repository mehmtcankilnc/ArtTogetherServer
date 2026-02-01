using ArtTogether.Domain.Entities;
using ArtTogether.Domain.Interfaces;
using MediatR;

namespace ArtTogether.Application.Features.Projects.Commands;

public record JoinProjectCommand(Guid ProjectId, Guid UserId) : IRequest<bool>;

public class JoinProjectCommandHandler(IProjectRepository repository) : IRequestHandler<JoinProjectCommand, bool>
{
    private readonly IProjectRepository _repository = repository;

    public async Task<bool> Handle(JoinProjectCommand request, CancellationToken cancellationToken)
    {
        var isMember = await _repository.IsMemberAsync(request.ProjectId, request.UserId);

        if (isMember) return true;

        var newMember = new ProjectMember
        {
            ProjectId = request.ProjectId,
            UserId = request.UserId,
            Role = ProjectRole.Editor,
        };

        await _repository.AddMemberAsync(newMember);
        return true;
    }
}
