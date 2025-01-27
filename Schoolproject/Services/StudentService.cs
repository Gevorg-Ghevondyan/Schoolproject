using Microsoft.EntityFrameworkCore;
using Schoolproject.Data;

public class StudentService : IStudentService
{
    private readonly SchoolContext _context;

    public StudentService(SchoolContext context)
    {
        _context = context;
    }

    public async Task<Student> CreateStudentAsync(StudentDTO studentDTO)
    {
        if (studentDTO == null) throw new ArgumentNullException(nameof(studentDTO));

        var classExists = await _context.Classes.AnyAsync(c => c.Id == studentDTO.ClassId);
        if (!classExists)
        {
            throw new Exception("Class not found");
        }

        var student = new Student
        {
            Name = studentDTO.Name,
            Email = studentDTO.Email,
            ClassId = studentDTO.ClassId
        };

        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        return student;
    }


    public async Task<StudentDTO?> GetStudentAsync(int id)
    {
        var student = await _context.Students
            .Include(s => s.Class)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (student == null)
        {
            return null;
        }

        return new StudentDTO
        {
            Name = student.Name ?? string.Empty,
            Email = student.Email ?? string.Empty,
            ClassId = student.ClassId ?? 0
        };
    }

    public async Task<List<Student>> GetAllStudentsAsync()
    {
        return await _context.Students
            .Include(s => s.Class)
            .ToListAsync();
    }

    public async Task<Student?> UpdateStudentAsync(int id, StudentDTO studentDTO)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
        {
            return null;
        }

        var classExists = await _context.Classes.AnyAsync(c => c.Id == studentDTO.ClassId);
        if (!classExists)
        {
            return null;
        }

        student.Name = studentDTO.Name;
        student.Email = studentDTO.Email;
        student.ClassId = studentDTO.ClassId;

        _context.Students.Update(student);
        await _context.SaveChangesAsync();

        return student;
    }

    public async Task<bool> DeleteStudentAsync(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
        {
            return false;
        }

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();

        return true;
    }
}