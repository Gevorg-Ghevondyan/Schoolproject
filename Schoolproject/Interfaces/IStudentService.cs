public interface IStudentService
{
    Task<Student> CreateStudentAsync(StudentDTO studentDTO);
    Task<StudentDTO> GetStudentAsync(int id);
    Task<List<Student>> GetAllStudentsAsync();
    Task<Student> UpdateStudentAsync(int id, StudentDTO studentDTO);
    Task<bool> DeleteStudentAsync(int id);
}
