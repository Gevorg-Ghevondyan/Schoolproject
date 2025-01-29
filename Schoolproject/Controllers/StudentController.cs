using Microsoft.AspNetCore.Mvc;
using Schoolproject.DTOs;

namespace Schoolproject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost]
        public async Task<ActionResult<StudentResponseDto>> CreateStudent(StudentRequestDTO studentDto)
        {
            var student = await _studentService.CreateAsync(studentDto);
            return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentResponseDto>>> GetAllStudents()
        {
            var students = await _studentService.GetAllAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentResponseDto>> GetStudentById(int id)
        {
            var student = await _studentService.GetByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<StudentResponseDto>> UpdateStudent(int id, StudentRequestDTO studentDto)
        {
            var updatedStudent = await _studentService.UpdateAsync(id, studentDto);
            if (updatedStudent == null)
            {
                return NotFound();
            }
            return Ok(updatedStudent);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStudent(int id)
        {
            var success = await _studentService.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}