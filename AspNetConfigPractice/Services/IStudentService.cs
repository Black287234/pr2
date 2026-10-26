using AspNetConfigPractice.Models;

namespace AspNetConfigPractice.Services;

public interface IStudentService
{
    // Список студентов с необязательным фильтром по курсу и сортировкой (name | course).
    Task<List<StudentDto>> GetAllAsync(int? course, string? sort);

    // null — если студент не найден.
    Task<StudentDto?> GetByIdAsync(int id);

    Task<StudentDto> CreateAsync(CreateStudentDto request);

    // false — если студент не найден.
    Task<bool> UpdateAsync(int id, UpdateStudentDto request);

    // false — если студент не найден.
    Task<bool> DeleteAsync(int id);
}
