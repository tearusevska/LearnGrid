using Domain.Common;
using Domain.Enums;

namespace Domain.Models;

public class Enrollment : BaseAuditableEntity
{
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;

    public Guid ClassId { get; set; }
    public Class Class { get; set; } = null!;

    public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;
    public decimal? FinalGrade { get; set; }
}