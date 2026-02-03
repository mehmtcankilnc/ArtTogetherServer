namespace ArtTogether.Application.DTOs;

public class ProjectDto
{
    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; }
    public string Width { get; set; }
    public string Height { get; set; }
    public string BackgroundColor { get; set; }
    public string InvitationUrl { get; set; }
    public string DeepLinkUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsOwner { get; set; }
}