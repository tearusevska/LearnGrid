using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<TeachingAssignment> TeachingAssignments { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<RecommendedResource> RecommendedResources { get; set; }
    public DbSet<EtlSyncLog> EtlSyncLogs { get; set; }
}