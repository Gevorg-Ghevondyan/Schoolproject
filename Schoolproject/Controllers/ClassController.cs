using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ClassController : ControllerBase
{
    private readonly IClassService _classService;

    public ClassController(IClassService classService)
    {
        _classService = classService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateClass([FromBody] ClassDTO classDTO)
    {
        if (classDTO == null)
            return BadRequest("Invalid class data.");

        var classEntity = await _classService.CreateClassAsync(classDTO);

        return CreatedAtAction(nameof(GetClass), new { id = classEntity.Id }, classEntity);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetClass(int id)
    {
        var classEntity = await _classService.GetClassAsync(id);
        if (classEntity == null)
            return NotFound();

        return Ok(classEntity);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllClasses()
    {
        var classes = await _classService.GetAllClassesAsync();
        return Ok(classes);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClass(int id, [FromBody] ClassDTO classDTO)
    {
        if (classDTO == null)
        {
            return BadRequest("Invalid class data.");
        }
        var updatedClass = await _classService.UpdateClassAsync(id, classDTO);
        if (updatedClass == null)
        {
            return NotFound(new { Message = "Class not found." });
        }
        return Ok(updatedClass);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClass(int id)
    {
        try
        {
            var success = await _classService.DeleteClassAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { Message = ex.Message });
        }
    }

}