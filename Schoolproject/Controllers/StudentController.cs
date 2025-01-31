using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Schoolproject.Data;

namespace Schoolproject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly SchoolContext _context;

        public StudentController(IStudentService studentService, SchoolContext context)
        {
            _studentService = studentService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] StudentRequestDTO studentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var student = await _studentService.CreateAsync(studentDto);
                return Ok(student);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _studentService.GetAllAsync();

            if (students == null || !students.Any())
            {
                return NotFound("No students found.");
            }

            return Ok(students);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid student ID. ID must be greater than zero.");
                }

                var student = await _studentService.GetByIdAsync(id);
                return Ok(student);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentRequestDTO studentDto)
        {
            try
            {

                var updatedStudent = await _studentService.UpdateAsync(id, studentDto);
                if (updatedStudent == null)
                {
                    return NotFound($"Student with ID {id} not found.");
                }

                if (string.IsNullOrWhiteSpace(studentDto.Name))
                {
                    return BadRequest("Student name cannot be empty.");
                }

                var existingStudentWithEmail = await _context.Students
                    .AnyAsync(s => s.Email == studentDto.Email && s.Id != id);

                if (existingStudentWithEmail)
                {
                    return Conflict("A student with this email already exists.");
                }

                if (studentDto.ClassId.HasValue)
                {
                    var classExists = await _context.Classes
                        .AnyAsync(c => c.Id == studentDto.ClassId.Value);

                    if (!classExists)
                    {
                        return BadRequest("The specified class does not exist.");
                    }
                }


                return Ok(updatedStudent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                var student = await _context.Students.FindAsync(id);
                if (student == null)
                {
                    return NotFound($"Student with ID {id} not found.");
                }

                var isAssignedToClass = await _context.Classes
                    .AnyAsync(c => c.StudentIds.Contains(id));

                if (isAssignedToClass)
                {
                    return BadRequest("Cannot delete the student because they are assigned to a class.");
                }

                var success = await _studentService.DeleteAsync(id);
                if (!success)
                {
                    return StatusCode(500, "Failed to delete student due to an internal error.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

    }
}