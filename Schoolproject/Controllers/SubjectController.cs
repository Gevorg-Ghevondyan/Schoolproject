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
    public async Task<IActionResult> CreateSubject([FromBody] SubjectDTO subjectDTO)
    {
        if (subjectDTO == null)
            return BadRequest("Invalid subject data.");

        var subject = await _subjectService.CreateSubjectAsync(subjectDTO);
        if (subject == null)
            return NotFound("One or more teachers not found.");

        return CreatedAtAction(nameof(GetSubject), new { id = subject.Id }, subject);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSubject(int id)
    {
        var subject = await _subjectService.GetSubjectAsync(id);
        if (subject == null)
            return NotFound();

        return Ok(subject);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSubjects()
    {
        var subjects = await _subjectService.GetAllSubjectsAsync();
        return Ok(subjects);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSubject(int id, [FromBody] SubjectDTO subjectDTO)
    {
        if (id <= 0 || subjectDTO == null)
            return BadRequest();

        var subject = await _subjectService.UpdateSubjectAsync(id, subjectDTO);
        if (subject == null)
            return NotFound();

        return Ok(subject);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubject(int id)
    {
        var success = await _subjectService.DeleteSubjectAsync(id);
        if (!success)
            return NotFound();

        return NoContent();
    }
}