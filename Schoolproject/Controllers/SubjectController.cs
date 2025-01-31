using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class SubjectController : ControllerBase
{
    private readonly ISubjectService _subjectService;

    public SubjectController(ISubjectService subjectService)
    {
        _subjectService = subjectService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSubject([FromBody] SubjectRequestDTO subjectDto)
    {
        try
        {
            var subject = await _subjectService.CreateAsync(subjectDto);
            return CreatedAtAction(nameof(GetSubjectById), new { id = subject.Id }, subject);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSubjects()
    {
        var subjects = await _subjectService.GetAllAsync();
        return Ok(subjects);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSubjectById(int id)
    {
        var subject = await _subjectService.GetByIdAsync(id);
        if (subject == null)
        {
            return NotFound($"Subject with ID {id} not found.");
        }
        return Ok(subject);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSubject(int id, [FromBody] SubjectRequestDTO subjectDto)
    {
        try
        {
            var updatedSubject = await _subjectService.UpdateAsync(id, subjectDto);
            if (updatedSubject == null)
                return NotFound($"Subject with ID {id} not found.");

            return Ok(updatedSubject);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubject(int id)
    {
        var deleted = await _subjectService.DeleteAsync(id);
        if (!deleted)
            return NotFound($"Subject with ID {id} not found.");

        return NoContent();
    }
}