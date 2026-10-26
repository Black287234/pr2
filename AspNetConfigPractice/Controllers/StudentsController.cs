using AspNetConfigPractice.Models;
using AspNetConfigPractice.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNetConfigPractice.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _students;

    public StudentsController(IStudentService students)
    {
        _students = students;
    }

    // GET /api/students
    // GET /api/students?course=2
    // GET /api/students?sort=name | sort=course
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentDto>>> GetAll(int? course, string? sort)
    {
        var result = await _students.GetAllAsync(course, sort);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<StudentDto>> GetById(int id)
    {
        var student = await _students.GetByIdAsync(id);
        if (student is null)
            return NotFound();

        return Ok(student);
    }

    [HttpPost]
    public async Task<ActionResult<StudentDto>> Create(CreateStudentDto request)
    {
        var created = await _students.CreateAsync(request);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateStudentDto request)
    {
        var updated = await _students.UpdateAsync(id, request);
        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _students.DeleteAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
