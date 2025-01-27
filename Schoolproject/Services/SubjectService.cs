using Microsoft.EntityFrameworkCore;
using Schoolproject.Data;

public class SubjectService : ISubjectService
{
    private readonly SchoolContext _context;

    public SubjectService(SchoolContext context)
    {
        _context = context;
    }

    public async Task<Subject> CreateSubjectAsync(SubjectDTO subjectDTO)
    {
        var validTeachers = await _context.Teachers
                                           .Where(t => subjectDTO.TeacherIds.Contains(t.Id))
                                           .ToListAsync();

        if (validTeachers.Count != subjectDTO.TeacherIds.Count)
            return null;

        var subject = new Subject
        {
            Name = subjectDTO.Name,
            Teachers = validTeachers
        };

        _context.Subjects.Add(subject);
        await _context.SaveChangesAsync();

        return subject;
    }

    public async Task<Subject> GetSubjectAsync(int id)
    {
        return await _context.Subjects
                             .Include(s => s.Teachers)
                             .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<Subject>> GetAllSubjectsAsync()
    {
        return await _context.Subjects
                             .Include(s => s.Teachers)
                             .ToListAsync();
    }

    public async Task<Subject> UpdateSubjectAsync(int id, SubjectDTO subjectDTO)
    {
        var subject = await _context.Subjects.FindAsync(id);
        if (subject == null)
            return null;

        var validTeachers = await _context.Teachers
                                           .Where(t => subjectDTO.TeacherIds.Contains(t.Id))
                                           .ToListAsync();

        if (validTeachers.Count != subjectDTO.TeacherIds.Count)
            return null;

        subject.Name = subjectDTO.Name;
        subject.Teachers = validTeachers;

        await _context.SaveChangesAsync();

        return subject;
    }

    public async Task<bool> DeleteSubjectAsync(int id)
    {
        var subject = await _context.Subjects.FindAsync(id);
        if (subject == null)
            return false;

        _context.Subjects.Remove(subject);
        await _context.SaveChangesAsync();

        return true;
    }
}