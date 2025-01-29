using Microsoft.EntityFrameworkCore;
using Schoolproject.Data;
using Schoolproject.DTOs;

namespace Schoolproject.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly SchoolContext _context;

        public SubjectService(SchoolContext context)
        {
            _context = context;
        }

        public async Task<SubjectResponseDto> CreateAsync(SubjectRequestDTO subjectDto)
        {
            var subject = new Subject
            {
                Name = subjectDto.Name,
                TeacherIds = subjectDto.TeacherIds ?? new List<int>()
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

        public async Task<SubjectResponseDto> GetByIdAsync(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) return null;

            return new SubjectResponseDto
            {
                Id = subject.Id,
                Name = subject.Name,
                TeacherIds = subject.TeacherIds
            };
        }

        public async Task<IEnumerable<SubjectResponseDto>> GetAllAsync()
        {
            return await _context.Subjects.Select(subject => new SubjectResponseDto
            {
                Id = subject.Id,
                Name = subject.Name,
                TeacherIds = subject.TeacherIds
            }).ToListAsync();
        }

        public async Task<SubjectResponseDto> UpdateAsync(int id, SubjectRequestDTO subjectDto)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) return null;

            subject.Name = subjectDto.Name;
            subject.TeacherIds = subjectDto.TeacherIds ?? subject.TeacherIds;

            await _context.SaveChangesAsync();

            return new SubjectResponseDto
            {
                Id = subject.Id,
                Name = subject.Name,
                TeacherIds = subject.TeacherIds
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) return false;

            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}