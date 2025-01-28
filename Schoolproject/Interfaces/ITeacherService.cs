public interface ITeacherService
{
    Task<TeacherDTO> CreateTeacherAsync(TeacherDTO teacherDTO);
    Task<List<Teacher>> GetAllTeachersAsync();
    Task<Teacher> GetTeacherAsync(int id);
    Task<TeacherDTO> UpdateTeacherAsync(int id, TeacherDTO teacherDTO);
    Task<bool> DeleteTeacherAsync(int id);
}