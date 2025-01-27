using Microsoft.AspNetCore.Mvc;

namespace Schoolproject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] StudentDTO studentDTO)
        {
            if (studentDTO == null)
            {
                return BadRequest("Invalid student data.");
            }

            try
            {
                var student = await _studentService.CreateStudentAsync(studentDTO);
                return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Class not found")
                {
                    return NotFound("Class not found.");
                }
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDTO>> GetStudent(int id)
        {
            var student = await _studentService.GetStudentAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return Ok(students);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentDTO studentDTO)
        {
            var student = await _studentService.UpdateStudentAsync(id, studentDTO);

            if (student == null)
            {
                var studentExists = await _studentService.GetStudentAsync(id);
                if (studentExists == null)
                {
                    return NotFound("Student not found.");
                }

                return NotFound("Class not found.");
            }
            return Ok(student);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var result = await _studentService.DeleteStudentAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}