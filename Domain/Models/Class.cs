using Domain.Common;

namespace Domain.Models;

public class Class : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string SchoolYear { get; set; } = string.Empty;
    public int MaxCapacity { get; set; }

    public ICollection<TeachingAssignment> TeachingAssignments { get; set; } = new List<TeachingAssignment>();
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}