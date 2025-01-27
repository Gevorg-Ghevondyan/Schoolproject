public interface ISubjectService
{
    Task<Subject> CreateSubjectAsync(SubjectDTO subjectDTO);

    Task<Subject> GetSubjectAsync(int id);

    Task<List<Subject>> GetAllSubjectsAsync();

    Task<Subject> UpdateSubjectAsync(int id, SubjectDTO subjectDTO);

    Task<bool> DeleteSubjectAsync(int id);
}