using Domain.Models;
using Microsoft.Extensions.Logging;
using Repository.Interface;
using Service.Interface;

namespace Service.Implementation;

public class ResourceEtlService : IResourceEtlService
{
    private readonly IRepository<Course> _courseRepository;
    private readonly IRepository<RecommendedResource> _resourceRepository;
    private readonly IRepository<EtlSyncLog> _syncLogRepository;
    private readonly IOpenLibraryApiClient _apiClient;
    private readonly ILogger<ResourceEtlService> _logger;

    public ResourceEtlService(
        IRepository<Course> courseRepository,
        IRepository<RecommendedResource> resourceRepository,
        IRepository<EtlSyncLog> syncLogRepository,
        IOpenLibraryApiClient apiClient,
        ILogger<ResourceEtlService> logger)
    {
        _courseRepository = courseRepository;
        _resourceRepository = resourceRepository;
        _syncLogRepository = syncLogRepository;
        _apiClient = apiClient;
        _logger = logger;
    }

    public async Task SyncRecommendedResourcesAsync()
    {
        var syncLog = new EtlSyncLog
        {
            SourceName = "OpenLibrary",
            StartedAt = DateTime.UtcNow,
            Status = "Running"
        };

        var processedCount = 0;

        try
        {
            var courses = await _courseRepository.GetAllAsync(x => x);

            foreach (var course in courses)
            {
                var existing = await _resourceRepository.GetAllAsync(
                    selector: x => x.Id,
                    predicate: x => x.CourseId == course.Id);

                if (existing.Any())
                    continue;

                var apiResult = await _apiClient.SearchByCategoryAsync(course.Category);

                var resources = apiResult.Docs
                    .Where(d => !string.IsNullOrWhiteSpace(d.Title))
                    .Select(d => new RecommendedResource
                    {
                        CourseId = course.Id,
                        Title = d.Title!,
                        Author = d.AuthorName?.FirstOrDefault(),
                        ThumbnailUrl = d.CoverId.HasValue
                            ? $"https://covers.openlibrary.org/b/id/{d.CoverId}-M.jpg"
                            : null,
                        Source = "OpenLibrary"
                    })
                    .ToList();

                if (resources.Count > 0)
                {
                    await _resourceRepository.InsertManyAsync(resources);
                    processedCount += resources.Count;
                }
            }

            syncLog.Status = "Success";
            syncLog.RecordsProcessed = processedCount;
            syncLog.FinishedAt = DateTime.UtcNow;

            _logger.LogInformation("ETL sync completed. {Count} resources inserted.", processedCount);
        }
        catch (Exception ex)
        {
            syncLog.Status = "Failed";
            syncLog.ErrorMessage = ex.Message;
            syncLog.FinishedAt = DateTime.UtcNow;

            _logger.LogError(ex, "ETL sync failed.");
        }
        finally
        {
            await _syncLogRepository.InsertAsync(syncLog);
        }
    }
}