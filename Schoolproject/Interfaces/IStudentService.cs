public interface IStudentService
{
    Task<StudentDTO> CreateStudentAsync(StudentDTO studentDTO);
    Task<Student> GetStudentAsync(int id);
    Task<List<Student>> GetAllStudentsAsync();
    Task<StudentDTO> UpdateStudentAsync(int id, StudentDTO studentDTO);
    Task<bool> DeleteStudentAsync(int id);
}