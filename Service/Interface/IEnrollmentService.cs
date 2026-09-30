using Domain.Dto;
using Domain.Models;
using Domain.Enums;

namespace Service.Interface;

public interface IEnrollmentService
{
    Task<List<EnrollmentResponseDto>> GetAllAsync();
    Task<EnrollmentResponseDto?> GetByIdAsync(Guid id);
    Task<Enrollment> InsertAsync(EnrollmentDto dto);
    Task<Enrollment> DeleteAsync(Guid id);
    Task<Enrollment> ChangeStatusAsync(Guid id, EnrollmentStatus status);
    Task<Enrollment> SetGradeAsync(Guid id, decimal finalGrade);
    Task<PaginatedResult<EnrollmentResponseDto>> GetAllPagedAsync(int pageNumber, int pageSize);
}