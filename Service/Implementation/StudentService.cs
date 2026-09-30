using Domain.Dto;
using Domain.Models;
using Repository.Interface;
using Service.Interface;

namespace Service.Implementation;

public class StudentService : IStudentService
{
    private readonly IRepository<Student> _repository;

    public StudentService(IRepository<Student> repository)
    {
        _repository = repository;
    }

    public async Task<List<Student>> GetAllAsync()
    {
        var result = await _repository.GetAllAsync(x => x);
        return result.ToList();
    }

    public async Task<Student?> GetByIdAsync(Guid id)
    {
        return await _repository.Get(
            selector: x => x,
            predicate: x => x.Id == id);
    }

    public async Task<Student> InsertAsync(StudentDto dto)
    {
        var student = new Student
        {
            FullName = dto.FullName,
            Email = dto.Email,
            EnrollmentDate = DateTime.UtcNow
        };

        return await _repository.InsertAsync(student);
    }

    public async Task<Student> UpdateAsync(Guid id, StudentDto dto)
    {
        var student = await GetByIdAsync(id);
        if (student == null)
            throw new InvalidOperationException($"Student with id {id} not found.");

        student.FullName = dto.FullName;
        student.Email = dto.Email;

        return await _repository.UpdateAsync(student);
    }

    public async Task<Student> DeleteAsync(Guid id)
    {
        var student = await GetByIdAsync(id);
        if (student == null)
            throw new InvalidOperationException($"Student with id {id} not found.");

        return await _repository.DeleteAsync(student);
    }

    public async Task<PaginatedResult<Student>> GetAllPagedAsync(int pageNumber, int pageSize)
    {
        return await _repository.GetAllPagedAsync(x => x, pageNumber, pageSize);
    }
}