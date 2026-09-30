namespace Service.Interface;

public interface IResourceEtlService
{
    Task SyncRecommendedResourcesAsync();
}