namespace Domain.Common;

public class BaseAuditableEntity : BaseEntity
{
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime? DateLastModified { get; set; }
}