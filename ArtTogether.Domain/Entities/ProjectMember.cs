namespace ArtTogether.Domain.Entities;

public class ProjectMember
{
    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public ProjectRole Role { get; set; }
}

public enum ProjectRole
{
    Owner = 0,
    Editor = 1,
}
