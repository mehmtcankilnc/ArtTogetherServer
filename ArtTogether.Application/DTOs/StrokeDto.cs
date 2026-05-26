namespace ArtTogether.Application.DTOs;

public class StrokeDto
{
    public Guid Id { get; set; }
    public string Color { get; set; }
    public float Width { get; set; }
    public string PathData { get; set; }
    public bool IsEraser { get; set; }
}
