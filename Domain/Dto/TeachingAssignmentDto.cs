namespace Domain.Dto;

public class TeachingAssignmentDto
{
    public Guid TeacherId { get; set; }
    public Guid CourseId { get; set; }
    public Guid ClassId { get; set; }
    public string Semester { get; set; } = string.Empty;
    public string? Room { get; set; }
    public string? ScheduleSlot { get; set; }
}