using Microsoft.EntityFrameworkCore;
using Schoolproject.Data;
using Schoolproject.DTOs;

namespace Schoolproject.Services
{
    public class StudentService : IStudentService
    {
        private readonly SchoolContext _context;

        public StudentService(SchoolContext context)
        {
            _context = context;
        }

        public async Task<StudentResponseDto> CreateAsync(StudentRequestDTO studentDto)
        {
            var student = new Student
            {
                Name = studentDto.Name,
                Email = studentDto.Email,
                ClassId = studentDto.ClassId
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return new StudentResponseDto
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                ClassId = student.ClassId
            };
        }

        public async Task<StudentResponseDto> GetByIdAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return null;

            return new StudentResponseDto
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                ClassId = student.ClassId
            };
        }

        public async Task<IEnumerable<StudentResponseDto>> GetAllAsync()
        {
            return await _context.Students.Select(student => new StudentResponseDto
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                ClassId = student.ClassId
            }).ToListAsync();
        }

        public async Task<StudentResponseDto> UpdateAsync(int id, StudentRequestDTO studentDto)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return null;

            student.Name = studentDto.Name;
            student.Email = studentDto.Email;
            student.ClassId = studentDto.ClassId;

            await _context.SaveChangesAsync();

            return new StudentResponseDto
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                ClassId = student.ClassId
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}