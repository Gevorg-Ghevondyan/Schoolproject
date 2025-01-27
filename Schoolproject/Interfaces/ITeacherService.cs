public interface ITeacherService
{
    Task<Teacher> CreateTeacherAsync(TeacherDTO teacherDTO);
    Task<Teacher> GetTeacherAsync(int id);
    Task<List<Teacher>> GetAllTeachersAsync();
    Task<Teacher> UpdateTeacherAsync(int id, TeacherDTO teacherDTO);
    Task<bool> DeleteTeacherAsync(int id);
}