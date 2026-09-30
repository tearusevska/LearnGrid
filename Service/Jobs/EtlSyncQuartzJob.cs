using Microsoft.Extensions.Logging;
using Quartz;
using Service.Interface;

namespace Service.Jobs;

public class EtlSyncQuartzJob : IJob
{
    private readonly IResourceEtlService _etlService;
    private readonly ILogger<EtlSyncQuartzJob> _logger;

    public EtlSyncQuartzJob(IResourceEtlService etlService, ILogger<EtlSyncQuartzJob> logger)
    {
        _etlService = etlService;
        _logger = logger;
    }

    public async ValueTask Execute(IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("ETL sync job started...");

        await _etlService.SyncRecommendedResourcesAsync();

        _logger.LogInformation("ETL sync job finished.");
    }
}