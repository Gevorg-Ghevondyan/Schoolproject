using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class TeacherController : ControllerBase
{
    private readonly ITeacherService _teacherService;

    public TeacherController(ITeacherService teacherService)
    {
        _teacherService = teacherService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTeacher([FromBody] TeacherDTO teacherDTO)
    {
        if (teacherDTO == null)
            return BadRequest("Invalid teacher data.");

        var teacher = await _teacherService.CreateTeacherAsync(teacherDTO);
        if (teacher == null)
            return NotFound("One or more subjects not found.");

        return CreatedAtAction(nameof(GetTeacher), new { id = teacher.Id }, teacher);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTeacher(int id)
    {
        var teacher = await _teacherService.GetTeacherAsync(id);
        if (teacher == null)
            return NotFound();

        return Ok(teacher);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTeachers()
    {
        var teachers = await _teacherService.GetAllTeachersAsync();
        return Ok(teachers);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTeacher(int id, [FromBody] TeacherDTO teacherDTO)
    {
        if (id <= 0 || teacherDTO == null)
            return BadRequest();

        var teacher = await _teacherService.UpdateTeacherAsync(id, teacherDTO);
        if (teacher == null)
            return NotFound();

        return Ok(teacher);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTeacher(int id)
    {
        var success = await _teacherService.DeleteTeacherAsync(id);
        if (!success)
            return NotFound();
        return NoContent();
    }
}