public interface ISubjectService
{
    Task<SubjectDTO> CreateSubjectAsync(SubjectDTO subjectDTO);
    Task<List<Subject>> GetAllSubjectsAsync();
    Task<Subject> GetSubjectAsync(int id);
    Task<SubjectDTO> UpdateSubjectAsync(int id, SubjectDTO subjectDTO);
    Task<bool> DeleteSubjectAsync(int id);
}