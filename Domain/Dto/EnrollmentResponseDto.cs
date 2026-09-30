using Domain.Enums;

namespace Domain.Dto;

public class EnrollmentResponseDto
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public Guid ClassId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public EnrollmentStatus Status { get; set; }
    public decimal? FinalGrade { get; set; }
    public DateTime DateCreated { get; set; }
}