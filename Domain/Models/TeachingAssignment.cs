using Domain.Common;

namespace Domain.Models;

public class TeachingAssignment : BaseEntity
{
    public Guid TeacherId { get; set; }
    public Teacher Teacher { get; set; } = null!;

    public Guid CourseId { get; set; }
    public Course Course { get; set; } = null!;

    public Guid ClassId { get; set; }
    public Class Class { get; set; } = null!;

    public string Semester { get; set; } = string.Empty;
    public string? Room { get; set; }
    public string? ScheduleSlot { get; set; } 
}