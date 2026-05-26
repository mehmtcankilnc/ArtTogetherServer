namespace ArtTogether.Domain.Entities;

public class Project
{
    public Guid Id { get; set; }
    public string ProjectName { get; set; }
    public string Width { get; set; } = "1920";
    public string Height { get; set; } = "1080";
    public string BackgroundColor { get; set; } = "#FFFFFF";
    public List<string> Swatches { get; set; } = ["", "", "", "", ""];
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid CreatedUserId { get; set; }
    public User CreatedBy { get; set; } = null!;

    // projenin küçük resmi ?
    public List<Stroke> Strokes { get; set; } = [];
    public List<ProjectMember> Members { get; set; } = [];
}
