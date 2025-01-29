using Schoolproject.DTOs;

public interface ITeacherService
{
    Task<TeacherResponseDto> CreateAsync(TeacherRequestDTO teacherDto);
    Task<TeacherResponseDto> GetByIdAsync(int id);
    Task<IEnumerable<TeacherResponseDto>> GetAllAsync();
    Task<TeacherResponseDto> UpdateAsync(int id, TeacherRequestDTO teacherDto);
    Task<bool> DeleteAsync(int id);
}