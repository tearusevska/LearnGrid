using Domain.Dto;
using Domain.Models;

namespace Service.Interface;

public interface ITeachingAssignmentService
{
    Task<List<TeachingAssignmentResponseDto>> GetAllAsync();
    Task<TeachingAssignmentResponseDto?> GetByIdAsync(Guid id);
    Task<TeachingAssignment> InsertAsync(TeachingAssignmentDto dto);
    Task<TeachingAssignment> UpdateAsync(Guid id, TeachingAssignmentDto dto);
    Task<TeachingAssignment> DeleteAsync(Guid id);
    Task<PaginatedResult<TeachingAssignmentResponseDto>> GetAllPagedAsync(int pageNumber, int pageSize);
}