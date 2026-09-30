using Domain.Dto;
using Domain.Models;

namespace Service.Interface;

public interface ICourseService
{
    Task<List<Course>> GetAllAsync();
    Task<Course?> GetByIdAsync(Guid id);
    Task<Course> InsertAsync(CourseDto dto);
    Task<Course> UpdateAsync(Guid id, CourseDto dto);
    Task<Course> DeleteAsync(Guid id);
    Task<PaginatedResult<Course>> GetAllPagedAsync(int pageNumber, int pageSize);
}