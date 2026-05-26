namespace ArtTogether.Application.DTOs;

public class ProjectUpdateDto
{
    public string? ProjectName { get; set; }
    public string? Width { get; set; }
    public string? Height { get; set; }
    public string? BackgroundColor { get; set; }
    public List<string>? Swatches { get; set; }
    public DateTime? LastUpdated { get; set; }
}
