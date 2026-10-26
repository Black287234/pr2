using AspNetConfigPractice.Data;
using AspNetConfigPractice.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetConfigPractice.Services;

public class StudentService : IStudentService
{
    private readonly AppDbContext _db;

    public StudentService(AppDbContext db)
    {
        _db = db;
    }

    private static StudentDto ToDto(Student student) =>
        new(student.Id, student.Name, student.Course);

    public async Task<List<StudentDto>> GetAllAsync(int? course, string? sort)
    {
        IQueryable<Student> query = _db.Students;

        if (course.HasValue)
            query = query.Where(s => s.Course == course.Value);

        // Неизвестное значение sort игнорируется — порядок остаётся по умолчанию.
        query = sort?.ToLowerInvariant() switch
        {
            "name" => query.OrderBy(s => s.Name),
            "course" => query.OrderBy(s => s.Course).ThenBy(s => s.Name),
            _ => query
        };

        return await query
            .Select(s => new StudentDto(s.Id, s.Name, s.Course))
            .ToListAsync();
    }

    public async Task<StudentDto?> GetByIdAsync(int id)
    {
        var student = await _db.Students.FindAsync(id);
        return student is null ? null : ToDto(student);
    }

    public async Task<StudentDto> CreateAsync(CreateStudentDto request)
    {
        var student = new Student
        {
            Name = request.Name,
            Course = request.Course
        };

        _db.Students.Add(student);
        await _db.SaveChangesAsync();

        return ToDto(student);
    }

    public async Task<bool> UpdateAsync(int id, UpdateStudentDto request)
    {
        var student = await _db.Students.FindAsync(id);
        if (student is null)
            return false;

        student.Name = request.Name;
        student.Course = request.Course;
        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var student = await _db.Students.FindAsync(id);
        if (student is null)
            return false;

        _db.Students.Remove(student);
        await _db.SaveChangesAsync();

        return true;
    }
}
