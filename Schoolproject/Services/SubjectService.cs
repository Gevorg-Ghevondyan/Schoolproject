using Microsoft.EntityFrameworkCore;
using Schoolproject.Data;
using Schoolproject.DTOs;

public class SubjectService : ISubjectService
{
    private readonly SchoolContext _context;

    public SubjectService(SchoolContext context)
    {
        _context = context;
    }

    public async Task<SubjectResponseDto> CreateAsync(SubjectRequestDTO subjectDto)
    {
        if (subjectDto == null)
            throw new ArgumentNullException(nameof(subjectDto));

        bool nameExists = await _context.Subjects
            .AnyAsync(s => s.Name.ToLower() == subjectDto.Name.ToLower());

        if (nameExists)
        {
            throw new ArgumentException($"The subject name '{subjectDto.Name}' already exists.");
        }

        var existingTeacherIds = await _context.Teachers
            .Where(t => subjectDto.TeacherIds.Contains(t.Id))
            .Select(t => t.Id)
            .ToListAsync();

        var invalidTeacherIds = subjectDto.TeacherIds.Except(existingTeacherIds).ToList();

        if (invalidTeacherIds.Any())
        {
            throw new ArgumentException($"The following teacher IDs do not exist: {string.Join(", ", invalidTeacherIds)}");
        }

        var subject = new Subject
        {
            Name = subjectDto.Name,
            TeacherIds = subjectDto.TeacherIds,
            Teachers = await _context.Teachers.Where(t => subjectDto.TeacherIds.Contains(t.Id)).ToListAsync()
        };

        _context.Subjects.Add(subject);
        await _context.SaveChangesAsync();

        return new SubjectResponseDto
        {
            Id = subject.Id,
            Name = subject.Name,
            TeacherIds = subject.TeacherIds
        };
    }
    public async Task<IEnumerable<SubjectResponseDto>> GetAllAsync()
    {
        var subjects = await _context.Subjects
            .Include(s => s.Teachers)
            .ToListAsync();

        return subjects.Select(s => new SubjectResponseDto
        {
            Id = s.Id,
            Name = s.Name,
            TeacherIds = s.Teachers.Select(t => t.Id).ToList()
        });
    }
    public async Task<SubjectResponseDto> GetByIdAsync(int id)
    {
        var subject = await _context.Subjects
            .Include(s => s.Teachers)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (subject == null)
            return null;

        return new SubjectResponseDto
        {
            Id = subject.Id,
            Name = subject.Name,
            TeacherIds = subject.Teachers.Select(t => t.Id).ToList()
        };
    }
    public async Task<SubjectResponseDto> UpdateAsync(int id, SubjectRequestDTO subjectDto)
    {
        var subject = await _context.Subjects
            .Include(s => s.Teachers)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (subject == null)
            return null;

        if (string.IsNullOrWhiteSpace(subjectDto.Name))
            throw new ArgumentException("Subject name cannot be empty.");

        var existingSubject = await _context.Subjects.AnyAsync(s => s.Name == subjectDto.Name && s.Id != id);
        if (existingSubject)
            throw new ArgumentException("A subject with this name already exists.");

        subject.Name = subjectDto.Name;

        subject.Teachers.Clear();
        if (subjectDto.TeacherIds != null && subjectDto.TeacherIds.Any())
        {
            var teachers = await _context.Teachers
                .Where(t => subjectDto.TeacherIds.Contains(t.Id))
                .ToListAsync();
            subject.Teachers.AddRange(teachers);
        }

        await _context.SaveChangesAsync();

        return new SubjectResponseDto
        {
            Id = subject.Id,
            Name = subject.Name,
            TeacherIds = subject.Teachers.Select(t => t.Id).ToList()
        };
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var subject = await _context.Subjects.FindAsync(id);
        if (subject == null)
            return false;

        _context.Subjects.Remove(subject);
        await _context.SaveChangesAsync();
        return true;
    }
}