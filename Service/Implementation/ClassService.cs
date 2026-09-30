using Domain.Dto;
using Domain.Models;
using Repository.Interface;
using Service.Interface;

namespace Service.Implementation;

public class ClassService : IClassService
{
    private readonly IRepository<Class> _repository;

    public ClassService(IRepository<Class> repository)
    {
        _repository = repository;
    }

    public async Task<List<Class>> GetAllAsync()
    {
        var result = await _repository.GetAllAsync(x => x);
        return result.ToList();
    }

    public async Task<Class?> GetByIdAsync(Guid id)
    {
        return await _repository.Get(
            selector: x => x,
            predicate: x => x.Id == id);
    }

    public async Task<Class> InsertAsync(ClassDto dto)
    {
        var cls = new Class
        {
            Name = dto.Name,
            SchoolYear = dto.SchoolYear,
            MaxCapacity = dto.MaxCapacity
        };

        return await _repository.InsertAsync(cls);
    }

    public async Task<Class> UpdateAsync(Guid id, ClassDto dto)
    {
        var cls = await GetByIdAsync(id);
        if (cls == null)
            throw new InvalidOperationException($"Class with id {id} not found.");

        cls.Name = dto.Name;
        cls.SchoolYear = dto.SchoolYear;
        cls.MaxCapacity = dto.MaxCapacity;

        return await _repository.UpdateAsync(cls);
    }

    public async Task<Class> DeleteAsync(Guid id)
    {
        var cls = await GetByIdAsync(id);
        if (cls == null)
            throw new InvalidOperationException($"Class with id {id} not found.");

        return await _repository.DeleteAsync(cls);
    }

    public async Task<PaginatedResult<Class>> GetAllPagedAsync(int pageNumber, int pageSize)
    {
        return await _repository.GetAllPagedAsync(x => x, pageNumber, pageSize);
    }
}