namespace ArtTogether.Application.DTOs;

public record CreatedProjectDto(
    Guid ProjectId,
    string ProjectName,
    string InvitationUrl,
    string DeepLinkUrl
);
