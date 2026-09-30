using Domain.Dto;
using Domain.Models;
using Repository.Interface;
using Service.Interface;

namespace Service.Implementation;

public class TeachingAssignmentService : ITeachingAssignmentService
{
    private readonly IRepository<TeachingAssignment> _repository;

    public TeachingAssignmentService(IRepository<TeachingAssignment> repository)
    {
        _repository = repository;
    }

    public async Task<List<TeachingAssignmentResponseDto>> GetAllAsync()
    {
        var result = await _repository.GetAllAsync(
            selector: x => new TeachingAssignmentResponseDto
            {
                Id = x.Id,
                TeacherId = x.TeacherId,
                TeacherName = x.Teacher.FullName,
                CourseId = x.CourseId,
                CourseTitle = x.Course.Title,
                ClassId = x.ClassId,
                ClassName = x.Class.Name,
                Semester = x.Semester,
                Room = x.Room,
                ScheduleSlot = x.ScheduleSlot
            });
        return result.ToList();
    }

    public async Task<TeachingAssignmentResponseDto?> GetByIdAsync(Guid id)
    {
        return await _repository.Get(
            selector: x => new TeachingAssignmentResponseDto
            {
                Id = x.Id,
                TeacherId = x.TeacherId,
                TeacherName = x.Teacher.FullName,
                CourseId = x.CourseId,
                CourseTitle = x.Course.Title,
                ClassId = x.ClassId,
                ClassName = x.Class.Name,
                Semester = x.Semester,
                Room = x.Room,
                ScheduleSlot = x.ScheduleSlot
            },
            predicate: x => x.Id == id);
    }

    public async Task<TeachingAssignment> InsertAsync(TeachingAssignmentDto dto)
    {
        await EnsureNoScheduleConflictAsync(dto, excludeId: null);

        var assignment = new TeachingAssignment
        {
            TeacherId = dto.TeacherId,
            CourseId = dto.CourseId,
            ClassId = dto.ClassId,
            Semester = dto.Semester,
            Room = dto.Room,
            ScheduleSlot = dto.ScheduleSlot
        };

        return await _repository.InsertAsync(assignment);
    }

    public async Task<TeachingAssignment> UpdateAsync(Guid id, TeachingAssignmentDto dto)
    {
        var assignment = await _repository.Get(x => x, predicate: x => x.Id == id);
        if (assignment == null)
            throw new InvalidOperationException($"TeachingAssignment with id {id} not found.");

        await EnsureNoScheduleConflictAsync(dto, excludeId: id);

        assignment.TeacherId = dto.TeacherId;
        assignment.CourseId = dto.CourseId;
        assignment.ClassId = dto.ClassId;
        assignment.Semester = dto.Semester;
        assignment.Room = dto.Room;
        assignment.ScheduleSlot = dto.ScheduleSlot;

        return await _repository.UpdateAsync(assignment);
    }

    public async Task<TeachingAssignment> DeleteAsync(Guid id)
    {
        var assignment = await _repository.Get(x => x, predicate: x => x.Id == id);
        if (assignment == null)
            throw new InvalidOperationException($"TeachingAssignment with id {id} not found.");

        return await _repository.DeleteAsync(assignment);
    }

    public async Task<PaginatedResult<TeachingAssignmentResponseDto>> GetAllPagedAsync(int pageNumber, int pageSize)
    {
        return await _repository.GetAllPagedAsync(
            selector: x => new TeachingAssignmentResponseDto
            {
                Id = x.Id,
                TeacherId = x.TeacherId,
                TeacherName = x.Teacher.FullName,
                CourseId = x.CourseId,
                CourseTitle = x.Course.Title,
                ClassId = x.ClassId,
                ClassName = x.Class.Name,
                Semester = x.Semester,
                Room = x.Room,
                ScheduleSlot = x.ScheduleSlot
            },
            pageNumber: pageNumber,
            pageSize: pageSize);
    }

    private async Task EnsureNoScheduleConflictAsync(TeachingAssignmentDto dto, Guid? excludeId)
    {
        if (string.IsNullOrWhiteSpace(dto.ScheduleSlot))
            return;

        var conflict = await _repository.Get(
            selector: x => x,
            predicate: x => x.TeacherId == dto.TeacherId
                          && x.Semester == dto.Semester
                          && x.ScheduleSlot == dto.ScheduleSlot
                          && (excludeId == null || x.Id != excludeId));

        if (conflict != null)
        {
            throw new InvalidOperationException(
                "Овој наставник веќе има закажан термин во истиот семестар и временски слот.");
        }
    }
}