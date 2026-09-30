using Domain.Common;

namespace Domain.Models;

public class EtlSyncLog : BaseEntity
{
    public string SourceName { get; set; } = string.Empty;
    public DateTime StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public int RecordsProcessed { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ErrorMessage { get; set; }
}