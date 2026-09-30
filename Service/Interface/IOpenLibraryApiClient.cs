using Domain.ExternalModels;

namespace Service.Interface;

public interface IOpenLibraryApiClient
{
    Task<OpenLibrarySearchResponse> SearchByCategoryAsync(string category, int limit = 5);
}