namespace ArtTogether.Domain.Entities;

public class Stroke
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string SessionId { get; set; }
    public string UserId { get; set; }
    public string Color { get; set; }
    public float Width { get; set; }
    public string PathData { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
