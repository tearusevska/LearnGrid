using Domain.Dto;
using Domain.Models;

namespace Service.Interface;

public interface IStudentService
{
    Task<List<Student>> GetAllAsync();
    Task<Student?> GetByIdAsync(Guid id);
    Task<Student> InsertAsync(StudentDto dto);
    Task<Student> UpdateAsync(Guid id, StudentDto dto);
    Task<Student> DeleteAsync(Guid id);
    Task<PaginatedResult<Student>> GetAllPagedAsync(int pageNumber, int pageSize);
}