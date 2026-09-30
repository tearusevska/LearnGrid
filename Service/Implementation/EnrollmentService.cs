using Domain.Dto;
using Domain.Dto.Email;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using Service.Interface;

namespace Service.Implementation;

public class EnrollmentService : IEnrollmentService
{
    private readonly IRepository<Enrollment> _repository;
    private readonly IRepository<Class> _classRepository;
    private readonly IRepository<Student> _studentRepository;
    private readonly IEmailQueue _emailQueue;

    public EnrollmentService(
        IRepository<Enrollment> repository,
        IRepository<Class> classRepository,
        IRepository<Student> studentRepository,
        IEmailQueue emailQueue)
    {
        _repository = repository;
        _classRepository = classRepository;
        _studentRepository = studentRepository;
        _emailQueue = emailQueue;
    }

    public async Task<List<EnrollmentResponseDto>> GetAllAsync()
    {
        var result = await _repository.GetAllAsync(
            selector: x => new EnrollmentResponseDto
            {
                Id = x.Id,
                StudentId = x.StudentId,
                StudentName = x.Student.FullName,
                ClassId = x.ClassId,
                ClassName = x.Class.Name,
                Status = x.Status,
                FinalGrade = x.FinalGrade,
                DateCreated = x.DateCreated
            });
        return result.ToList();
    }

    public async Task<EnrollmentResponseDto?> GetByIdAsync(Guid id)
    {
        return await _repository.Get(
            selector: x => new EnrollmentResponseDto
            {
                Id = x.Id,
                StudentId = x.StudentId,
                StudentName = x.Student.FullName,
                ClassId = x.ClassId,
                ClassName = x.Class.Name,
                Status = x.Status,
                FinalGrade = x.FinalGrade,
                DateCreated = x.DateCreated
            },
            predicate: x => x.Id == id);
    }

    public async Task<Enrollment> InsertAsync(EnrollmentDto dto)
    {
        await EnsureClassHasCapacityAsync(dto.ClassId);

        var student = await _studentRepository.Get(x => x, predicate: x => x.Id == dto.StudentId);
        if (student == null)
            throw new InvalidOperationException($"Student with id {dto.StudentId} not found.");

        var cls = await _classRepository.Get(x => x, predicate: x => x.Id == dto.ClassId);
        if (cls == null)
            throw new InvalidOperationException($"Class with id {dto.ClassId} not found.");

        var enrollment = new Enrollment
        {
            StudentId = dto.StudentId,
            ClassId = dto.ClassId,
            Status = EnrollmentStatus.Active,
            FinalGrade = null
        };

        var result = await _repository.InsertAsync(enrollment);

        await _emailQueue.EnqueueAsync(new EmailMessage
        {
            To = student.Email,
            Subject = $"Успешно запишување – {cls.Name}",
            HtmlBody = $"<p>Здраво {student.FullName},</p><p>Успешно сте запишани во паралелката {cls.Name} за учебната {cls.SchoolYear}.</p>"
        });

        return result;
    }

    public async Task<Enrollment> DeleteAsync(Guid id)
    {
        var enrollment = await _repository.Get(x => x, predicate: x => x.Id == id);
        if (enrollment == null)
            throw new InvalidOperationException($"Enrollment with id {id} not found.");

        return await _repository.DeleteAsync(enrollment);
    }

    public async Task<Enrollment> ChangeStatusAsync(Guid id, EnrollmentStatus status)
    {
        var enrollment = await _repository.Get(x => x, predicate: x => x.Id == id);
        if (enrollment == null)
            throw new InvalidOperationException($"Enrollment with id {id} not found.");

        if (status == EnrollmentStatus.Completed)
            throw new InvalidOperationException(
                "Статусот 'Completed' се поставува после внесување на оцена.");

        enrollment.Status = status;
        return await _repository.UpdateAsync(enrollment);
    }

    public async Task<Enrollment> SetGradeAsync(Guid id, decimal finalGrade)
    {
        if (finalGrade < 1 || finalGrade > 5)
            throw new InvalidOperationException("Оценката мора да биде од 1 до 5.");

        var enrollment = await _repository.Get(x => x, predicate: x => x.Id == id);
        if (enrollment == null)
            throw new InvalidOperationException($"Enrollment with id {id} not found.");

        if (enrollment.Status != EnrollmentStatus.Active)
            throw new InvalidOperationException(
                "Оценка може да се внесе само за активно запишување (Status = Active).");

        enrollment.FinalGrade = finalGrade;
        if (finalGrade == 1)
        {
            enrollment.Status = EnrollmentStatus.Dropped;
        }
        else
        {
            enrollment.Status = EnrollmentStatus.Completed;   
        }

        return await _repository.UpdateAsync(enrollment);
    }

    public async Task<PaginatedResult<EnrollmentResponseDto>> GetAllPagedAsync(int pageNumber, int pageSize)
    {
        return await _repository.GetAllPagedAsync(
            selector: x => new EnrollmentResponseDto
            {
                Id = x.Id,
                StudentId = x.StudentId,
                StudentName = x.Student.FullName,
                ClassId = x.ClassId,
                ClassName = x.Class.Name,
                Status = x.Status,
                FinalGrade = x.FinalGrade,
                DateCreated = x.DateCreated
            },
            pageNumber: pageNumber,
            pageSize: pageSize);
    }

    private async Task EnsureClassHasCapacityAsync(Guid classId)
    {
        var cls = await _classRepository.Get(x => x, predicate: x => x.Id == classId);
        if (cls == null)
            throw new InvalidOperationException($"Class with id {classId} not found.");

        var activeCount = (await _repository.GetAllAsync(
            selector: x => x.Id,
            predicate: x => x.ClassId == classId && x.Status == EnrollmentStatus.Active)).Count();

        if (activeCount >= cls.MaxCapacity)
        {
            throw new InvalidOperationException(
                $"Класата '{cls.Name}' го достигна максималниот капацитет ({cls.MaxCapacity}).");
        }
    }
}