namespace ArtTogether.Domain.Entities;

public class Project
{
    public Guid Id { get; set; }
    public string ProjectName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid CreatedUserId { get; set; }
    public User CreatedBy { get; set; } = null!;

    // projenin küçük resmi ?
    public List<Stroke> Strokes { get; set; } = [];
    public List<ProjectMember> Members { get; set; } = [];
}
