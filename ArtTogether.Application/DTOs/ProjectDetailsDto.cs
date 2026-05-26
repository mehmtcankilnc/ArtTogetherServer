namespace ArtTogether.Application.DTOs;

public class ProjectDetailsDto : ProjectDto
{
    public string Width { get; set; }
    public string Height { get; set; }
    public string BackgroundColor { get; set; }
    public List<string> Swatches { get; set; }
}
