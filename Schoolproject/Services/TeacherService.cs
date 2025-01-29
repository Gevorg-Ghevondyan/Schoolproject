using Microsoft.EntityFrameworkCore;
using Schoolproject.Data;
using Schoolproject.DTOs;

namespace Schoolproject.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly SchoolContext _context;

        public TeacherService(SchoolContext context)
        {
            _context = context;
        }

        public async Task<TeacherResponseDto> CreateAsync(TeacherRequestDTO teacherDto)
        {
            var teacher = new Teacher
            {
                Name = teacherDto.Name,
                Email = teacherDto.Email,
                SubjectIds = teacherDto.SubjectIds ?? new List<int>(),
                ClassIds = teacherDto.ClassIds ?? new List<int>()
            };

            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();

            return new TeacherResponseDto
            {
                Id = teacher.Id,
                Name = teacher.Name,
                Email = teacher.Email,
                SubjectIds = teacher.SubjectIds,
                ClassIds = teacher.ClassIds
            };
        }

        public async Task<TeacherResponseDto> GetByIdAsync(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return null;

            return new TeacherResponseDto
            {
                Id = teacher.Id,
                Name = teacher.Name,
                Email = teacher.Email,
                SubjectIds = teacher.SubjectIds,
                ClassIds = teacher.ClassIds
            };
        }

        public async Task<IEnumerable<TeacherResponseDto>> GetAllAsync()
        {
            return await _context.Teachers.Select(teacher => new TeacherResponseDto
            {
                Id = teacher.Id,
                Name = teacher.Name,
                Email = teacher.Email,
                SubjectIds = teacher.SubjectIds,
                ClassIds = teacher.ClassIds
            }).ToListAsync();
        }

        public async Task<TeacherResponseDto> UpdateAsync(int id, TeacherRequestDTO teacherDto)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return null;

            teacher.Name = teacherDto.Name;
            teacher.Email = teacherDto.Email;
            teacher.SubjectIds = teacherDto.SubjectIds ?? teacher.SubjectIds;
            teacher.ClassIds = teacherDto.ClassIds ?? teacher.ClassIds;

            await _context.SaveChangesAsync();

            return new TeacherResponseDto
            {
                Id = teacher.Id,
                Name = teacher.Name,
                Email = teacher.Email,
                SubjectIds = teacher.SubjectIds,
                ClassIds = teacher.ClassIds
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return false;

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}