using Domain.Common;

namespace Domain.Models;

public class RecommendedResource : BaseEntity
{
    public Guid CourseId { get; set; }
    public Course Course { get; set; } = null!;

    public string Title { get; set; } = string.Empty;
    public string? Author { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string Source { get; set; } = "OpenLibrary";
}