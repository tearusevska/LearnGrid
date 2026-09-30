namespace Domain.Dto;

public class CourseDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Category { get; set; } = string.Empty;
    public int DurationInHours { get; set; }
}