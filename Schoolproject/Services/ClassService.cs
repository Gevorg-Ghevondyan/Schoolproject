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
            // Check if a class with the same name already exists
            var existingClass = await _context.Classes
                .FirstOrDefaultAsync(c => c.Name.Equals(classDto.Name, StringComparison.OrdinalIgnoreCase));

            if (existingClass != null)
            {
                throw new ArgumentException($"A class with the name '{classDto.Name}' already exists.");
            }

            // Ensure no duplicate teacher or student IDs
            if (classDto.TeacherIds != null && classDto.TeacherIds.Distinct().Count() != classDto.TeacherIds.Count())
            {
                throw new ArgumentException("Teacher IDs cannot contain duplicates.");
            }

            if (classDto.StudentIds != null && classDto.StudentIds.Distinct().Count() != classDto.StudentIds.Count())
            {
                throw new ArgumentException("Student IDs cannot contain duplicates.");
            }

            // Validate TeacherIds and StudentIds
            if (classDto.TeacherIds != null && classDto.TeacherIds.Any())
            {
                var teacherIdsNotFound = classDto.TeacherIds
                    .Where(t => !_context.Teachers.Any(teacher => teacher.Id == t))
                    .ToList();

                if (teacherIdsNotFound.Any())
                {
                    throw new ArgumentException($"The following Teacher IDs are invalid: {string.Join(", ", teacherIdsNotFound)}");
                }
            }

            if (classDto.StudentIds != null && classDto.StudentIds.Any())
            {
                var studentIdsNotFound = classDto.StudentIds
                    .Where(s => !_context.Students.Any(student => student.Id == s))
                    .ToList();

                if (studentIdsNotFound.Any())
                {
                    throw new ArgumentException($"The following Student IDs are invalid: {string.Join(", ", studentIdsNotFound)}");
                }

                // Check if any student is already assigned to another class
                var studentsAlreadyInOtherClass = await _context.Classes
                    .AnyAsync(c => c.StudentIds.Any(studentId => classDto.StudentIds.Contains(studentId)));

                if (studentsAlreadyInOtherClass)
                {
                    throw new ArgumentException("One or more students are already assigned to another class.");
                }
            }

            // Create the new class entity
            var newClassEntity = new Class
            {
                Name = classDto.Name,
                TeacherIds = classDto.TeacherIds ?? new List<int>(),
                StudentIds = classDto.StudentIds ?? new List<int>()
            };

            // Add the new class to the database
            _context.Classes.Add(newClassEntity);
            await _context.SaveChangesAsync();

            // Return the response DTO
            return new ClassResponseDto
            {
                Id = newClassEntity.Id,
                Name = newClassEntity.Name,
                TeacherIds = newClassEntity.TeacherIds,
                StudentIds = newClassEntity.StudentIds
            };
        }
        public async Task<IEnumerable<ClassResponseDto>> GetAllAsync()
        {
            var classes = await _context.Classes
                                         .Select(c => new ClassResponseDto
                                         {
                                             Id = c.Id,
                                             Name = c.Name,
                                             SubjectIds = c.SubjectIds ?? new List<int>(),  // Ensure SubjectIds is not null
                                             TeacherIds = c.TeacherIds ?? new List<int>(),  // Ensure TeacherIds is not null
                                             StudentIds = c.StudentIds ?? new List<int>()   // Ensure StudentIds is not null
                                         })
                                         .ToListAsync();

            return classes;
        }
        public async Task<ClassResponseDto> GetByIdAsync(int id)
        {
            var classEntity = await _context.Classes.FindAsync(id);
            if (classEntity == null)
            {
                throw new KeyNotFoundException($"Class with ID {id} was not found.");
            }

            return new ClassResponseDto
            {
                Id = classEntity.Id,
                Name = classEntity.Name,
                TeacherIds = classEntity.TeacherIds,
                StudentIds = classEntity.StudentIds
            };
        }
        public async Task<ClassResponseDto> UpdateAsync(int id, ClassRequestDTO classDto)
        {
            var classEntity = await _context.Classes.FindAsync(id);
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

            // Teacher ID validation
            if (classDto.TeacherIds != null && classDto.TeacherIds.Distinct().Count() != classDto.TeacherIds.Count())
            {
                throw new ArgumentException("Teacher IDs cannot contain duplicates.");
            }

            // Student ID validation
            if (classDto.StudentIds != null && classDto.StudentIds.Distinct().Count() != classDto.StudentIds.Count())
            {
                throw new ArgumentException("Student IDs cannot contain duplicates.");
            }

            // Subject ID validation
            if (classDto.SubjectIds != null && classDto.SubjectIds.Any())
            {
                var invalidSubjectIds = classDto.SubjectIds
                    .Except(await _context.Subjects.Select(s => s.Id).ToListAsync())
                    .ToList();

                if (invalidSubjectIds.Any())
                {
                    throw new ArgumentException($"The following Subject IDs are invalid: {string.Join(", ", invalidSubjectIds)}");
                }
            }

            // Teacher ID existence check
            if (classDto.TeacherIds != null && classDto.TeacherIds.Any())
            {
                var invalidTeacherIds = classDto.TeacherIds
                    .Except(await _context.Teachers.Select(t => t.Id).ToListAsync())
                    .ToList();

                if (invalidTeacherIds.Any())
                {
                    throw new ArgumentException($"The following Teacher IDs are invalid: {string.Join(", ", invalidTeacherIds)}");
                }
            }

            // Student ID existence check
            if (classDto.StudentIds != null && classDto.StudentIds.Any())
            {
                var invalidStudentIds = classDto.StudentIds
                    .Except(await _context.Students.Select(s => s.Id).ToListAsync())
                    .ToList();

                if (invalidStudentIds.Any())
                {
                    throw new ArgumentException($"The following Student IDs are invalid: {string.Join(", ", invalidStudentIds)}");
                }

                // Check if any of the students are already assigned to another class
                var studentsAlreadyInOtherClass = await _context.Classes
                    .Where(c => c.StudentIds.Any(studentId => classDto.StudentIds.Contains(studentId)) && c.Id != id)
                    .AnyAsync();

                if (studentsAlreadyInOtherClass)
                {
                    throw new ArgumentException("One or more students are already assigned to another class.");
                }
            }

            // Update the class entity with the new data
            classEntity.Name = classDto.Name;
            classEntity.TeacherIds = classDto.TeacherIds ?? classEntity.TeacherIds;
            classEntity.StudentIds = classDto.StudentIds ?? classEntity.StudentIds;
            classEntity.SubjectIds = classDto.SubjectIds ?? classEntity.SubjectIds;

            try
            {
                // Save changes to the database
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while updating the class. Please try again later.", ex);
            }

            // Return the updated class response
            return new ClassResponseDto
            {
                Id = classEntity.Id,
                Name = classEntity.Name,
                TeacherIds = classEntity.TeacherIds,
                StudentIds = classEntity.StudentIds,
                SubjectIds = classEntity.SubjectIds
            };
        }
        public async Task<bool> DeleteAsync(int id)
        {
            // Step 1: Check if the class with the specified ID exists
            var classEntity = await _context.Classes.FindAsync(id);

            if (classEntity == null)
            {
                // If the class does not exist, return false and indicate failure
                return false; // Class not found, deletion failed
            }

            // Step 2: Check if any students are still assigned to the class
            if (classEntity.StudentIds.Any())
            {
                // Prevent deletion if students are assigned
                return false;
            }

            // Step 3: Check if any teachers are still assigned to the class
            if (classEntity.TeacherIds.Any())
            {
                // Prevent deletion if teachers are assigned
                return false;
            }

            // Step 4: Optionally check if subjectIds exist in other classes
            var otherClassWithSubject = await _context.Classes
                .AnyAsync(c => c.SubjectIds.Any(s => classEntity.SubjectIds.Contains(s)) && c.Id != id);

            if (otherClassWithSubject)
            {
                // Prevent deletion if subjects are assigned to other classes
                return false;
            }

            // Step 5: Delete the class
            _context.Classes.Remove(classEntity);

            try
            {
                await _context.SaveChangesAsync();
                return true; // Successfully deleted
            }
            catch (Exception)
            {
                // Return false in case of any exception during deletion
                return false;
            }
        }
    }
}