using Microsoft.EntityFrameworkCore;
using Schoolproject.Data;

public class ClassService : IClassService
{
    private readonly SchoolContext _context;

    public ClassService(SchoolContext context)
    {
        _context = context;
    }

    public async Task<Class> CreateClassAsync(ClassDTO classDTO)
    {
        if (classDTO.Name.Length > 10)
        {
            throw new ArgumentException("The Name field must be a string with a maximum length of 10 characters.");
        }
        if (_context.Classes.Any(c => c.Name == classDTO.Name))
        {
            throw new InvalidOperationException("Class with this name already exists.");
        }

        var studentIds = classDTO.StudentIds ?? new List<int>();
        var teacherIds = classDTO.TeacherIds ?? new List<int>();
        var subjectIds = classDTO.SubjectIds ?? new List<int>();

        var validStudentIds = await _context.Students
            .Where(s => studentIds.Contains(s.Id))
            .Select(s => s.Id)
            .ToListAsync();

        var invalidStudentIds = studentIds.Except(validStudentIds).ToList();
        if (invalidStudentIds.Any())
        {
            throw new InvalidOperationException($"Invalid student ID(s): {string.Join(", ", invalidStudentIds)}.");
        }

        var validTeacherIds = await _context.Teachers
            .Where(t => teacherIds.Contains(t.Id))
            .Select(t => t.Id)
            .ToListAsync();

        var invalidTeacherIds = teacherIds.Except(validTeacherIds).ToList();
        if (invalidTeacherIds.Any())
        {
            throw new InvalidOperationException($"Invalid teacher ID(s): {string.Join(", ", invalidTeacherIds)}.");
        }

        var validSubjectIds = await _context.Subjects
            .Where(sub => subjectIds.Contains(sub.Id))
            .Select(sub => sub.Id)
            .ToListAsync();

        var invalidSubjectIds = subjectIds.Except(validSubjectIds).ToList();
        if (invalidSubjectIds.Any())
        {
            throw new InvalidOperationException($"Invalid subject ID(s): {string.Join(", ", invalidSubjectIds)}.");
        }

        var newClass = new Class
        {
            Name = classDTO.Name,
            StudentIds = validStudentIds,
            TeacherIds = validTeacherIds,
            SubjectIds = validSubjectIds
        };

        _context.Classes.Add(newClass);
        await _context.SaveChangesAsync();

        return newClass;
    }

    public async Task<List<Class>> GetAllClassesAsync()
    {
        return await _context.Classes
            .Select(c => new Class
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToListAsync();
    }

    public async Task<Class> GetClassAsync(int id)
    {
        var classEntity = await _context.Classes
            .Include(c => c.Students)
            .Include(c => c.Teachers)
            .Include(c => c.Subjects)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (classEntity == null)
        {
            return null;
        }

        classEntity.StudentIds = classEntity.Students.Select(s => s.Id).ToList();
        classEntity.TeacherIds = classEntity.Teachers.Select(t => t.Id).ToList();
        classEntity.SubjectIds = classEntity.Subjects.Select(s => s.Id).ToList();

        return classEntity;
    }

    public async Task<Class> UpdateClassAsync(int id, ClassDTO classDTO)
    {
        var classEntity = await _context.Classes.FindAsync(id);
        if (classEntity == null)
        {
            throw new KeyNotFoundException($"Class with ID {id} not found.");
        }

        if (_context.Classes.Any(c => c.Name == classDTO.Name && c.Id != id))
        {
            throw new InvalidOperationException("Class with this name already exists.");
        }

        var studentIds = classDTO.StudentIds ?? new List<int>();
        var teacherIds = classDTO.TeacherIds ?? new List<int>();
        var subjectIds = classDTO.SubjectIds ?? new List<int>();

        var validStudentIds = await _context.Students
            .Where(s => studentIds.Contains(s.Id))
            .Select(s => s.Id)
            .ToListAsync();

        var invalidStudentIds = studentIds.Except(validStudentIds).ToList();
        if (invalidStudentIds.Any())
        {
            throw new InvalidOperationException($"Invalid student ID(s): {string.Join(", ", invalidStudentIds)}.");
        }

        var validTeacherIds = await _context.Teachers
            .Where(t => teacherIds.Contains(t.Id))
            .Select(t => t.Id)
            .ToListAsync();

        var invalidTeacherIds = teacherIds.Except(validTeacherIds).ToList();
        if (invalidTeacherIds.Any())
        {
            throw new InvalidOperationException($"Invalid teacher ID(s): {string.Join(", ", invalidTeacherIds)}.");
        }

        var validSubjectIds = await _context.Subjects
            .Where(sub => subjectIds.Contains(sub.Id))
            .Select(sub => sub.Id)
            .ToListAsync();

        var invalidSubjectIds = subjectIds.Except(validSubjectIds).ToList();
        if (invalidSubjectIds.Any())
        {
            throw new InvalidOperationException($"Invalid subject ID(s): {string.Join(", ", invalidSubjectIds)}.");
        }

        classEntity.Name = classDTO.Name;
        classEntity.StudentIds = validStudentIds;
        classEntity.TeacherIds = validTeacherIds;
        classEntity.SubjectIds = validSubjectIds;

        await _context.SaveChangesAsync();

        return classEntity;
    }

    public async Task<bool> DeleteClassAsync(int id)
    {
        var classEntity = await _context.Classes
            .Include(c => c.Students)
            .Include(c => c.Teachers)
            .Include(c => c.Subjects)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (classEntity == null)
        {
            throw new KeyNotFoundException($"Class with ID {id} not found.");
        }

        if (classEntity.Students.Any())
        {
            throw new InvalidOperationException("Cannot delete class. It has related students.");
        }

        if (classEntity.Teachers.Any())
        {
            throw new InvalidOperationException("Cannot delete class. It has related teachers.");
        }

        if (classEntity.Subjects.Any())
        {
            throw new InvalidOperationException("Cannot delete class. It has related subjects.");
        }

        _context.Classes.Remove(classEntity);

        await _context.SaveChangesAsync();

        return true;
    }
}