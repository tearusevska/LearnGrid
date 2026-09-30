using Domain.Dto;
using Domain.Models;

namespace Service.Interface;

public interface IClassService
{
    Task<List<Class>> GetAllAsync();
    Task<Class?> GetByIdAsync(Guid id);
    Task<Class> InsertAsync(ClassDto dto);
    Task<Class> UpdateAsync(Guid id, ClassDto dto);
    Task<Class> DeleteAsync(Guid id);
    Task<PaginatedResult<Class>> GetAllPagedAsync(int pageNumber, int pageSize);
}