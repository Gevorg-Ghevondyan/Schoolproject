using Microsoft.AspNetCore.Mvc;
using Schoolproject.DTOs;

namespace Schoolproject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly IClassService _classService;

        public ClassesController(IClassService classService)
        {
            _classService = classService;
        }

        [HttpPost]
        public async Task<ActionResult<ClassResponseDto>> CreateClass(ClassRequestDTO classDto)
        {
            try
            {
                var classEntity = await _classService.CreateAsync(classDto);
                return CreatedAtAction(nameof(GetClassById), new { id = classEntity.Id }, classEntity);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassResponseDto>>> GetAllClasses()
        {
            var classes = await _classService.GetAllAsync();
            return Ok(classes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClassById(int id)
        {
            try
            {
                var classDto = await _classService.GetByIdAsync(id);
                return Ok(classDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClass(int id, ClassRequestDTO classDto)
        {
            try
            {
                var updatedClass = await _classService.UpdateAsync(id, classDto);

                return Ok(updatedClass);
            }
            catch (ArgumentException ex)
            {

                if (ex.Message.Contains("not found"))
                {
                    return NotFound(new { message = ex.Message });
                }

                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClass(int id)
        {
            try
            {
                await _classService.DeleteAsync(id);
                return Ok(new { message = "Class deleted successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

    }
}