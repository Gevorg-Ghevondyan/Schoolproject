using Schoolproject.DTOs;
public interface IStudentService
{
    Task<StudentResponseDto> CreateAsync(StudentRequestDTO studentDto);
    Task<StudentResponseDto> GetByIdAsync(int id);
    Task<IEnumerable<StudentResponseDto>> GetAllAsync();
    Task<StudentResponseDto> UpdateAsync(int id, StudentRequestDTO studentDto);
    Task<bool> DeleteAsync(int id);
}