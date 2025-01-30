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
            var existingStudent = await _context.Students
                .FirstOrDefaultAsync(s => s.Email == studentDto.Email);

            if (existingStudent != null)
            {
                throw new ArgumentException("A student with this email already exists.");
            }

            if (studentDto.ClassId.HasValue)
            {
                var classEntity = await _context.Classes
                    .Include(c => c.Students)
                    .FirstOrDefaultAsync(c => c.Id == studentDto.ClassId.Value);

                if (classEntity == null)
                {
                    throw new KeyNotFoundException("The specified class does not exist.");
                }

                var studentEntity = new Student
                {
                    Name = studentDto.Name,
                    Email = studentDto.Email,
                    ClassId = studentDto.ClassId
                };

                classEntity.Students.Add(studentEntity);

                _context.Students.Add(studentEntity);
                await _context.SaveChangesAsync();

                return new StudentResponseDto
                {
                    Id = studentEntity.Id,
                    Name = studentEntity.Name,
                    Email = studentEntity.Email,
                    ClassId = studentEntity.ClassId
                };
            }

            throw new ArgumentException("ClassId must be provided for student creation.");
        }

        public async Task<StudentResponseDto> GetByIdAsync(int id)
        {
            var studentEntity = await _context.Students
                .Where(s => s.Id == id)
                .Select(s => new StudentResponseDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email,
                    ClassId = s.ClassId
                })
                .FirstOrDefaultAsync();

            if (studentEntity == null)
            {
                throw new ArgumentException($"Student with ID {id} not found.");
            }

            return studentEntity;
        }

        public async Task<IEnumerable<StudentResponseDto>> GetAllAsync()
        {
            var studentEntities = await _context.Students
                .Select(s => new StudentResponseDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email,
                    ClassId = s.ClassId
                })
                .ToListAsync();

            return studentEntities;
        }

        public async Task<StudentResponseDto> UpdateAsync(int id, StudentRequestDTO studentDto)
        {
            var studentEntity = await _context.Students
                .Include(s => s.Class)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (studentEntity == null)
            {
                throw new ArgumentException("Student not found.");
            }

            if (studentDto.ClassId.HasValue)
            {
                var classEntity = await _context.Classes
                    .Include(c => c.Students)
                    .FirstOrDefaultAsync(c => c.Id == studentDto.ClassId.Value);

                if (classEntity == null)
                {
                    throw new ArgumentException("The specified class does not exist.");
                }

                if (studentEntity.ClassId.HasValue && studentEntity.ClassId != studentDto.ClassId.Value)
                {
                    var oldClassEntity = await _context.Classes
                        .Include(c => c.Students)
                        .FirstOrDefaultAsync(c => c.Id == studentEntity.ClassId.Value);

                    if (oldClassEntity != null)
                    {
                        oldClassEntity.Students.Remove(studentEntity);
                        _context.Entry(oldClassEntity).State = EntityState.Modified;
                    }
                }

                classEntity.Students.Add(studentEntity);

                _context.Entry(classEntity).State = EntityState.Modified;

                studentEntity.ClassId = studentDto.ClassId;
            }
            else
            {
                studentEntity.ClassId = null;
            }

            studentEntity.Name = studentDto.Name;
            studentEntity.Email = studentDto.Email;

            await _context.SaveChangesAsync();

            return new StudentResponseDto
            {
                Id = studentEntity.Id,
                Name = studentEntity.Name,
                Email = studentEntity.Email,
                ClassId = studentEntity.ClassId
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var studentEntity = await _context.Students.FindAsync(id);

            if (studentEntity == null)
            {
                throw new ArgumentException($"Student with ID {id} not found.");
            }

            var studentInClass = await _context.Classes
                .Where(c => c.StudentIds.Contains(studentEntity.Id))
                .AnyAsync();

            if (studentInClass)
            {
                throw new InvalidOperationException("Student cannot be deleted because they are still enrolled in a class.");
            }

            _context.Students.Remove(studentEntity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}