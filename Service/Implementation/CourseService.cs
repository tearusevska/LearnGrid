using Domain.Dto;
using Domain.Models;
using Repository.Interface;
using Service.Interface;

namespace Service.Implementation;

public class CourseService : ICourseService
{
    private readonly IRepository<Course> _repository;

    public CourseService(IRepository<Course> repository)
    {
        _repository = repository;
    }

    public async Task<List<Course>> GetAllAsync()
    {
        var result = await _repository.GetAllAsync(x => x);
        return result.ToList();
    }

    public async Task<Course?> GetByIdAsync(Guid id)
    {
        return await _repository.Get(
            selector: x => x,
            predicate: x => x.Id == id);
    }

    public async Task<Course> InsertAsync(CourseDto dto)
    {
        var course = new Course
        {
            Title = dto.Title,
            Description = dto.Description,
            Category = dto.Category,
            DurationInHours = dto.DurationInHours
        };

        return await _repository.InsertAsync(course);
    }

    public async Task<Course> UpdateAsync(Guid id, CourseDto dto)
    {
        var course = await GetByIdAsync(id);
        if (course == null)
            throw new InvalidOperationException($"Course with id {id} not found.");

        course.Title = dto.Title;
        course.Description = dto.Description;
        course.Category = dto.Category;
        course.DurationInHours = dto.DurationInHours;

        return await _repository.UpdateAsync(course);
    }

    public async Task<Course> DeleteAsync(Guid id)
    {
        var course = await GetByIdAsync(id);
        if (course == null)
            throw new InvalidOperationException($"Course with id {id} not found.");

        return await _repository.DeleteAsync(course);
    }

    public async Task<PaginatedResult<Course>> GetAllPagedAsync(int pageNumber, int pageSize)
    {
        return await _repository.GetAllPagedAsync(x => x, pageNumber, pageSize);
    }
}