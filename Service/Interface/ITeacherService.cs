using Domain.Dto;
using Domain.Models;

namespace Service.Interface;

public interface ITeacherService
{
    Task<List<Teacher>> GetAllAsync();
    Task<Teacher?> GetByIdAsync(Guid id);
    Task<Teacher> InsertAsync(TeacherDto dto);
    Task<Teacher> UpdateAsync(Guid id, TeacherDto dto);
    Task<Teacher> DeleteAsync(Guid id);
    Task<PaginatedResult<Teacher>> GetAllPagedAsync(int pageNumber, int pageSize);
}