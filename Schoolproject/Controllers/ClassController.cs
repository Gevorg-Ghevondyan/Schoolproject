using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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
        if (classDTO == null || string.IsNullOrEmpty(classDTO.Name))
        {
            return BadRequest("Invalid class data.");
        }
        if (classDTO.Name.Length > 10)
        {
            return BadRequest(new { Message = "The Name field must be a string with a maximum length of 10 characters." });
        }

        try
        {
            var createdClass = await _classService.CreateClassAsync(classDTO);
            return CreatedAtAction(nameof(GetClass), new { id = createdClass.Id }, new
            {
                Id = createdClass.Id,
                Name = createdClass.Name
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetClass(int id)
    {
        var classEntity = await _classService.GetClassAsync(id);

        if (classEntity == null)
        {
            return NotFound($"Class with ID {id} not found.");
        }

        var classDto = new ClassDTO
        {
            Name = classEntity.Name
        };

        var classWithIds = new
        {
            classEntity.Id,
            classEntity.Name,
            StudentIds = classEntity.StudentIds,
            TeacherIds = classEntity.TeacherIds,
            SubjectIds = classEntity.SubjectIds
        };

        return Ok(classWithIds);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllClasses()
    {
        var classes = await _classService.GetAllClassesAsync();

        var classDtos = classes.Select(c => new
        {
            c.Id,
            c.Name,
            TeacherIds=c.TeacherIds,
            SubjectIds=c.SubjectIds,
            StudentIds=c.StudentIds
        }).ToList();

        return Ok(classDtos);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClass(int id, [FromBody] ClassDTO classDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var updatedClass = await _classService.UpdateClassAsync(id, classDTO);
            return Ok(updatedClass);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { Message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClass(int id)
    {
        try
        {
            var success = await _classService.DeleteClassAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }
}