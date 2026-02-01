namespace ArtTogether.Domain.Entities;

public class Stroke
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public string Color { get; set; }
    public float Width { get; set; }
    public string PathData { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public StrokeType Type { get; set; } = StrokeType.Brush;
}

public enum StrokeType { Brush, Eraser }
