using AspNetConfigPractice.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetConfigPractice.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private static readonly List<Student> Students = new()
    {
        new Student { Id = 1, Name = "Анна", Course = 2 },
        new Student { Id = 2, Name = "Илья", Course = 3 },
        new Student { Id = 3, Name = "Сергей", Course = 1 } // Уже добавлено по мини-заданию
    };

    private static int _nextId = 4;

    private static StudentDto ToDto(Student student) =>
        new(student.Id, student.Name, student.Course);

    [HttpGet]
    public ActionResult<IEnumerable<StudentDto>> GetAll()
    {
        return Ok(Students.Select(ToDto));
    }

    [HttpGet("{id:int}")]
    public ActionResult<StudentDto> GetById(int id)
    {
        var student = Students.FirstOrDefault(s => s.Id == id);
        if (student is null)
            return NotFound();

        return Ok(ToDto(student));
    }

    [HttpPost]
    public ActionResult<StudentDto> Create(CreateStudentDto request)
    {
        var student = new Student
        {
            Id = _nextId++,
            Name = request.Name,
            Course = request.Course
        };

        Students.Add(student);

        return CreatedAtAction(nameof(GetById), new { id = student.Id }, ToDto(student));
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, UpdateStudentDto request)
    {
        var student = Students.FirstOrDefault(s => s.Id == id);
        if (student is null)
            return NotFound();

        student.Name = request.Name;
        student.Course = request.Course;

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var student = Students.FirstOrDefault(s => s.Id == id);
        if (student is null)
            return NotFound();

        Students.Remove(student);

        return NoContent();
    }
}
