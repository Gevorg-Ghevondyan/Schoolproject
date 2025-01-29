using Microsoft.AspNetCore.Mvc;
using Schoolproject.DTOs;

namespace Schoolproject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectsController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpPost]
        public async Task<ActionResult<SubjectResponseDto>> CreateSubject(SubjectRequestDTO subjectDto)
        {
            var subject = await _subjectService.CreateAsync(subjectDto);
            return CreatedAtAction(nameof(GetSubjectById), new { id = subject.Id }, subject);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubjectResponseDto>>> GetAllSubjects()
        {
            var subjects = await _subjectService.GetAllAsync();
            return Ok(subjects);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SubjectResponseDto>> GetSubjectById(int id)
        {
            var subject = await _subjectService.GetByIdAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            return Ok(subject);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SubjectResponseDto>> UpdateSubject(int id, SubjectRequestDTO subjectDto)
        {
            var updatedSubject = await _subjectService.UpdateAsync(id, subjectDto);
            if (updatedSubject == null)
            {
                return NotFound();
            }
            return Ok(updatedSubject);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSubject(int id)
        {
            var success = await _subjectService.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}