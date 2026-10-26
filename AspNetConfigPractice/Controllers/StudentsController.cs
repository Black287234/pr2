using AspNetConfigPractice.Data;
using AspNetConfigPractice.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetConfigPractice.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly AppDbContext _db;

    public StudentsController(AppDbContext db)
    {
        _db = db;
    }

    private static StudentDto ToDto(Student student) =>
        new(student.Id, student.Name, student.Course);

    // GET /api/students
    // GET /api/students?course=2
    // GET /api/students?sort=name | sort=course
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentDto>>> GetAll(int? course, string? sort)
    {
        IQueryable<Student> query = _db.Students;

        if (course.HasValue)
            query = query.Where(s => s.Course == course.Value);

        if (!string.IsNullOrWhiteSpace(sort))
        {
            switch (sort.ToLowerInvariant())
            {
                case "name":
                    query = query.OrderBy(s => s.Name);
                    break;
                case "course":
                    query = query.OrderBy(s => s.Course).ThenBy(s => s.Name);
                    break;
                default:
                    return BadRequest("Параметр sort может быть только 'name' или 'course'.");
            }
        }

        var result = await query
            .Select(s => new StudentDto(s.Id, s.Name, s.Course))
            .ToListAsync();

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<StudentDto>> GetById(int id)
    {
        var student = await _db.Students.FindAsync(id);
        if (student is null)
            return NotFound();

        return Ok(ToDto(student));
    }

    [HttpPost]
    public async Task<ActionResult<StudentDto>> Create(CreateStudentDto request)
    {
        var student = new Student
        {
            Name = request.Name,
            Course = request.Course
        };

        _db.Students.Add(student);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = student.Id }, ToDto(student));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateStudentDto request)
    {
        var student = await _db.Students.FindAsync(id);
        if (student is null)
            return NotFound();

        student.Name = request.Name;
        student.Course = request.Course;
        await _db.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var student = await _db.Students.FindAsync(id);
        if (student is null)
            return NotFound();

        _db.Students.Remove(student);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
