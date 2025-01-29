using Microsoft.AspNetCore.Mvc;
using Schoolproject.DTOs;

namespace Schoolproject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeachersController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpPost]
        public async Task<ActionResult<TeacherResponseDto>> CreateTeacher(TeacherRequestDTO teacherDto)
        {
            var teacher = await _teacherService.CreateAsync(teacherDto);
            return CreatedAtAction(nameof(GetTeacherById), new { id = teacher.Id }, teacher);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeacherResponseDto>>> GetAllTeachers()
        {
            var teachers = await _teacherService.GetAllAsync();
            return Ok(teachers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TeacherResponseDto>> GetTeacherById(int id)
        {
            var teacher = await _teacherService.GetByIdAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return Ok(teacher);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TeacherResponseDto>> UpdateTeacher(int id, TeacherRequestDTO teacherDto)
        {
            var updatedTeacher = await _teacherService.UpdateAsync(id, teacherDto);
            if (updatedTeacher == null)
            {
                return NotFound();
            }
            return Ok(updatedTeacher);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTeacher(int id)
        {
            var success = await _teacherService.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
