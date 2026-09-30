using Domain.Common;

namespace Domain.Models;

public class Course : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Category { get; set; } = string.Empty;
    public int DurationInHours { get; set; }

    public ICollection<TeachingAssignment> TeachingAssignments { get; set; } = new List<TeachingAssignment>();
    public ICollection<RecommendedResource> RecommendedResources { get; set; } = new List<RecommendedResource>();
}