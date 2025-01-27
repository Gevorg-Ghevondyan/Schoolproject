using Microsoft.EntityFrameworkCore;
using Schoolproject.Data;
public class TeacherService : ITeacherService
{
    private readonly SchoolContext _context;

    public TeacherService(SchoolContext context)
    {
        _context = context;
    }

    public async Task<Teacher> CreateTeacherAsync(TeacherDTO teacherDTO)
    {
        var validSubjects = await _context.Subjects
                                           .Where(s => teacherDTO.SubjectIds.Contains(s.Id))
                                           .ToListAsync();

        if (validSubjects.Count != teacherDTO.SubjectIds.Count)
            return null;

        var teacher = new Teacher
        {
            Name = teacherDTO.Name,
            SubjectIds = teacherDTO.SubjectIds
        };

        _context.Teachers.Add(teacher);
        await _context.SaveChangesAsync();

        return teacher;
    }

    public async Task<Teacher> GetTeacherAsync(int id)
    {
        return await _context.Teachers
                             .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<List<Teacher>> GetAllTeachersAsync()
    {
        return await _context.Teachers.ToListAsync();
    }

    public async Task<Teacher> UpdateTeacherAsync(int id, TeacherDTO teacherDTO)
    {
        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher == null)
            return null;

        var validSubjects = await _context.Subjects
                                           .Where(s => teacherDTO.SubjectIds.Contains(s.Id))
                                           .ToListAsync();

        if (validSubjects.Count != teacherDTO.SubjectIds.Count)
            return null;

        teacher.Name = teacherDTO.Name;
        teacher.SubjectIds = teacherDTO.SubjectIds;

        await _context.SaveChangesAsync();

        return teacher;
    }

    public async Task<bool> DeleteTeacherAsync(int id)
    {
        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher == null)
            return false;

        _context.Teachers.Remove(teacher);
        await _context.SaveChangesAsync();

        return true;
    }
}