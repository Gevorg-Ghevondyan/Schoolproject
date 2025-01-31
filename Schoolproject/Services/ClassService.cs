using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Schoolproject.Data;
using Schoolproject.DTOs;

namespace Schoolproject.Services
{
    public class ClassService : IClassService
    {
        private readonly SchoolContext _context;

        public ClassService(SchoolContext context)
        {
            _context = context;
        }

        public async Task<ClassResponseDto> CreateAsync(ClassRequestDTO classDto)
        {
            var existingClass = await _context.Classes
                .Where(c => c.Name.ToLower() == classDto.Name.ToLower())
                .FirstOrDefaultAsync();


            if (existingClass != null)
            {
                throw new ArgumentException($"A class with the name '{classDto.Name}' already exists.");
            }

            if (classDto.TeacherIds != null && classDto.TeacherIds.Distinct().Count() != classDto.TeacherIds.Count())
            {
                throw new ArgumentException("Teacher IDs cannot contain duplicates.");
            }

            if (classDto.StudentIds != null && classDto.StudentIds.Distinct().Count() != classDto.StudentIds.Count())
            {
                throw new ArgumentException("Student IDs cannot contain duplicates.");
            }

            if (classDto.SubjectIds != null && classDto.SubjectIds.Distinct().Count() != classDto.SubjectIds.Count())
            {
                throw new ArgumentException("Subject IDs cannot contain duplicates.");
            }

            if (classDto.TeacherIds != null && classDto.TeacherIds.Any())
            {
                var validTeacherIds = await _context.Teachers.Where(t => classDto.TeacherIds.Contains(t.Id)).Select(t => t.Id).ToListAsync();
                var teacherIdsNotFound = classDto.TeacherIds.Except(validTeacherIds).ToList();

                if (teacherIdsNotFound.Any())
                {
                    throw new ArgumentException($"The following Teacher IDs are invalid: {string.Join(", ", teacherIdsNotFound)}");
                }
            }

            if (classDto.SubjectIds != null && classDto.SubjectIds.Any())
            {
                var validSubjectIds = await _context.Teachers.Where(t => classDto.SubjectIds.Contains(t.Id)).Select(t => t.Id).ToListAsync();
                var subjectIdsNotFound = classDto.SubjectIds.Except(validSubjectIds).ToList();

                if (subjectIdsNotFound.Any())
                {
                    throw new ArgumentException($"The following Subject IDs are invalid: {string.Join(", ", subjectIdsNotFound)}");
                }
            }

            if (classDto.StudentIds != null && classDto.StudentIds.Any())
            {
                var validStudentIds = await _context.Students.Where(s => classDto.StudentIds.Contains(s.Id)).Select(s => s.Id).ToListAsync();
                var studentIdsNotFound = classDto.StudentIds.Except(validStudentIds).ToList();

                if (studentIdsNotFound.Any())
                {
                    throw new ArgumentException($"The following Student IDs are invalid: {string.Join(", ", studentIdsNotFound)}");
                }

                var studentsAlreadyInOtherClass = await _context.Classes
                    .AnyAsync(c => c.Students.Any(student => classDto.StudentIds.Contains(student.Id)));

                if (studentsAlreadyInOtherClass)
                {
                    throw new ArgumentException("One or more students are already assigned to another class.");
                }
            }

            var newClassEntity = new Class
            {
                Name = classDto.Name,
                Teachers = classDto.TeacherIds != null ? await _context.Teachers.Where(t => classDto.TeacherIds.Contains(t.Id)).ToListAsync() : new List<Teacher>(),
                Students = classDto.StudentIds != null ? await _context.Students.Where(s => classDto.StudentIds.Contains(s.Id)).ToListAsync() : new List<Student>(),
                Subjects = classDto.SubjectIds != null ? await _context.Subjects.Where(s => classDto.SubjectIds.Contains(s.Id)).ToListAsync() : new List<Subject>()
            };

            _context.Classes.Add(newClassEntity);
            await _context.SaveChangesAsync();

            return new ClassResponseDto
            {
                Id = newClassEntity.Id,
                Name = newClassEntity.Name,
                TeacherIds = newClassEntity.Teachers.Select(t => t.Id).ToList(),
                StudentIds = newClassEntity.Students.Select(s => s.Id).ToList(),
                SubjectIds = newClassEntity.Subjects.Select(s => s.Id).ToList()
            };
        }
        public async Task<IEnumerable<ClassResponseDto>> GetAllAsync()
        {
            var classEntities = await _context.Classes
                .Include(c => c.Students)
                .Select(c => new ClassResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    StudentIds = c.Students.Select(s => s.Id).ToList(),
                    SubjectIds = c.Subjects.Select(s => s.Id).ToList(),
                    TeacherIds = c.Teachers.Select(t => t.Id).ToList()
                })
                .ToListAsync();

            return classEntities;
        }
        public async Task<ClassResponseDto> GetByIdAsync(int id)
        {
            var classEntity = await _context.Classes
                .Include(c => c.Students)
                .Include(c => c.Subjects)
                .Include(c => c.Teachers)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (classEntity == null)
            {
                throw new KeyNotFoundException($"Class with ID {id} was not found.");
            }

            return new ClassResponseDto
            {
                Id = classEntity.Id,
                Name = classEntity.Name,
                StudentIds = classEntity.Students.Select(s => s.Id).ToList(),
                SubjectIds = classEntity.Subjects.Select(s => s.Id).ToList(),
                TeacherIds = classEntity.Teachers.Select(t => t.Id).ToList()
            };
        }
        public async Task<ClassResponseDto> UpdateAsync(int id, ClassRequestDTO classDto)
        {
            var classEntity = await _context.Classes
                .Include(c => c.Students)
                .Include(c => c.Teachers)
                .Include(c => c.Subjects)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (classEntity == null)
            {
                throw new ArgumentException($"Class with ID {id} not found.");
            }

            var existingClass = await _context.Classes
                .Where(c => c.Name.ToLower() == classDto.Name.ToLower() && c.Id != id)
                .FirstOrDefaultAsync();

            if (existingClass != null)
            {
                throw new ArgumentException($"A class with the name '{classDto.Name}' already exists.");
            }

            if (classDto.TeacherIds != null && classDto.TeacherIds.Distinct().Count() != classDto.TeacherIds.Count())
            {
                throw new ArgumentException("Teacher IDs cannot contain duplicates.");
            }

            if (classDto.StudentIds != null && classDto.StudentIds.Distinct().Count() != classDto.StudentIds.Count())
            {
                throw new ArgumentException("Student IDs cannot contain duplicates.");
            }

            if (classDto.SubjectIds != null)
            {
                classEntity.Subjects.Clear();
                if (classDto.SubjectIds.Any())
                {
                    var newSubjects = await _context.Subjects
                        .Where(s => classDto.SubjectIds.Contains(s.Id))
                        .ToListAsync();
                    classEntity.Subjects.AddRange(newSubjects);
                }
            }

            if (classDto.TeacherIds != null)
            {
                classEntity.Teachers.Clear();
                if (classDto.TeacherIds.Any())
                {
                    var newTeachers = await _context.Teachers
                        .Where(t => classDto.TeacherIds.Contains(t.Id))
                        .ToListAsync();
                    classEntity.Teachers.AddRange(newTeachers);
                }
            }

            if (classDto.StudentIds != null)
            {
                classEntity.Students.Clear();
                if (classDto.StudentIds.Any())
                {
                    var validStudentIds = await _context.Students.Select(s => s.Id).ToListAsync();
                    var invalidStudentIds = classDto.StudentIds.Except(validStudentIds).ToList();

                    if (invalidStudentIds.Any())
                    {
                        throw new ArgumentException($"The following Student IDs are invalid: {string.Join(", ", invalidStudentIds)}");
                    }

                    var newStudents = await _context.Students
                        .Where(s => classDto.StudentIds.Contains(s.Id))
                        .ToListAsync();
                    classEntity.Students.AddRange(newStudents);
                }
            }

            classEntity.Name = classDto.Name;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while updating the class. Please try again later.", ex);
            }

            return new ClassResponseDto
            {
                Id = classEntity.Id,
                Name = classEntity.Name,
                TeacherIds = classEntity.Teachers.Select(t => t.Id).ToList(),
                StudentIds = classEntity.Students.Select(s => s.Id).ToList(),
                SubjectIds = classEntity.Subjects.Select(s => s.Id).ToList()
            };
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var classEntity = await _context.Classes
                .Include(c => c.Students)
                .Include(c => c.Teachers)
                .Include(c => c.Subjects)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (classEntity == null)
            {
                throw new KeyNotFoundException("Class with the provided ID not found.");
            }

            if (classEntity.Students.Any())
            {
                throw new InvalidOperationException("Cannot delete class because it still has assigned students.");
            }

            if (classEntity.Teachers.Any())
            {
                throw new InvalidOperationException("Cannot delete class because it still has assigned teachers.");
            }

            bool otherClassWithSubject = await _context.Classes
                .AnyAsync(c => c.Subjects.Any(s => classEntity.Subjects.Contains(s)) && c.Id != id);

            if (otherClassWithSubject)
            {
                throw new InvalidOperationException("Cannot delete class because its subjects are linked to other classes.");
            }

            _context.Classes.Remove(classEntity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}