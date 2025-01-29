using Schoolproject.DTOs;

public interface ISubjectService
{
    Task<SubjectResponseDto> CreateAsync(SubjectRequestDTO subjectDto);
    Task<SubjectResponseDto> GetByIdAsync(int id);
    Task<IEnumerable<SubjectResponseDto>> GetAllAsync();
    Task<SubjectResponseDto> UpdateAsync(int id, SubjectRequestDTO subjectDto);
    Task<bool> DeleteAsync(int id);
}