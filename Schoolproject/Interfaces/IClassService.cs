using Schoolproject.DTOs;

public interface IClassService
{
    Task<ClassResponseDto> CreateAsync(ClassRequestDTO classDto);
    Task<ClassResponseDto> GetByIdAsync(int id);
    Task<IEnumerable<ClassResponseDto>> GetAllAsync();
    Task<ClassResponseDto> UpdateAsync(int id, ClassRequestDTO classDto);
    Task<bool> DeleteAsync(int id);
}