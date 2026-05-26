using ArtTogether.Application.DTOs;
using ArtTogether.Domain.Entities;
using ArtTogether.Domain.Interfaces;
using MediatR;

namespace ArtTogether.Application.Features.Projects.Commands;

public record UpdateProjectCommand(Guid ProjectId, Guid UserId, ProjectUpdateDto Dto) : IRequest<ProjectDetailsDto>;

public class UpdateProjectCommandHandler(IProjectRepository repository) : IRequestHandler<UpdateProjectCommand, ProjectDetailsDto>
{
    private readonly IProjectRepository _repository = repository;

    public async Task<ProjectDetailsDto> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetProjectByIdAsync(request.ProjectId)
                     ?? throw new Exception("Proje bulunamadı.");

        await ValidatePermissionsAsync(entity, request.UserId, request.Dto);

        if (entity.CreatedUserId == request.UserId)
        {
            ApplyUpdates(entity, request.Dto);
        }

        entity.UpdatedAt = DateTime.UtcNow;
        await _repository.SaveChangesAsync();

        return MapToDto(entity, request.UserId);
    }

    private async Task ValidatePermissionsAsync(Project entity, Guid userId, ProjectUpdateDto dto)
    {
        bool isOwner = entity.CreatedUserId == userId;

        if (isOwner) return;

        if (!await _repository.IsMemberAsync(entity.Id, userId))
        {
            throw new UnauthorizedAccessException("Bu projeyi güncelleme yetkiniz yok.");
        }

        if (HasRestrictedChanges(dto))
        {
            throw new Exception("Proje ayarlarını sadece proje sahibi değiştirebilir.");
        }
    }

    private static bool HasRestrictedChanges(ProjectUpdateDto dto)
    {
        return dto.ProjectName != null ||
               dto.Width != null ||
               dto.Height != null ||
               dto.BackgroundColor != null;
    }

    private static void ApplyUpdates(Project entity, ProjectUpdateDto dto)
    {
        entity.ProjectName = dto.ProjectName ?? entity.ProjectName;
        entity.Width = dto.Width ?? entity.Width;
        entity.Height = dto.Height ?? entity.Height;
        entity.BackgroundColor = dto.BackgroundColor ?? entity.BackgroundColor;

        if (dto.Swatches != null)
        {
            entity.Swatches = new List<string>(dto.Swatches);
        }
    }

    private static ProjectDetailsDto MapToDto(Project entity, Guid currentUserId)
    {
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
            IsOwner = entity.CreatedUserId == currentUserId
        };
    }
}
