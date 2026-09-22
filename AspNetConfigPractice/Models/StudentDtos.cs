using System.ComponentModel.DataAnnotations;

namespace AspNetConfigPractice.Models;

public record StudentDto(int Id, string Name, int Course);

public record CreateStudentDto(
    [property: Required] string Name,
    [property: Range(1, 4)] int Course
);

public record UpdateStudentDto(
    [property: Required] string Name,
    [property: Range(1, 4)] int Course
);