using Domain.Dto;
using Domain.Models;
using Repository.Interface;
using Service.Interface;

namespace Service.Implementation;

public class TeacherService : ITeacherService
{
    private readonly IRepository<Teacher> _repository;

    public TeacherService(IRepository<Teacher> repository)
    {
        _repository = repository;
    }

    public async Task<List<Teacher>> GetAllAsync()
    {
        var result = await _repository.GetAllAsync(x => x);
        return result.ToList();
    }

    public async Task<Teacher?> GetByIdAsync(Guid id)
    {
        return await _repository.Get(
            selector: x => x,
            predicate: x => x.Id == id);
    }

    public async Task<Teacher> InsertAsync(TeacherDto dto)
    {
        var teacher = new Teacher
        {
            FullName = dto.FullName,
            Email = dto.Email,
            Title = dto.Title,
            Bio = dto.Bio
        };

        return await _repository.InsertAsync(teacher);
    }

    public async Task<Teacher> UpdateAsync(Guid id, TeacherDto dto)
    {
        var teacher = await GetByIdAsync(id);
        if (teacher == null)
        {
            throw new InvalidOperationException($"Teacher with id {id} not found.");
        }

        teacher.FullName = dto.FullName;
        teacher.Email = dto.Email;
        teacher.Title = dto.Title;
        teacher.Bio = dto.Bio;

        return await _repository.UpdateAsync(teacher);
    }

    public async Task<Teacher> DeleteAsync(Guid id)
    {
        var teacher = await GetByIdAsync(id);
        if (teacher == null)
        {
            throw new InvalidOperationException($"Teacher with id {id} not found.");
        }

        return await _repository.DeleteAsync(teacher);
    }

    public async Task<PaginatedResult<Teacher>> GetAllPagedAsync(int pageNumber, int pageSize)
    {
        return await _repository.GetAllPagedAsync(
            selector: x => x,
            pageNumber: pageNumber,
            pageSize: pageSize);
    }
}