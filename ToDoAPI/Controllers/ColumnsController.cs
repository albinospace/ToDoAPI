using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoAPI.DTOs.Columns;
using ToDoAPI.Services;

namespace ToDoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColumnsController : ControllerBase
    {
        private readonly IColumnService _columnService;

        public ColumnsController(IColumnService columnService)
        {
            _columnService = columnService;
        }

        [HttpGet("by-project/{projectId}")]
        public async Task<ActionResult<IEnumerable<ColumnResponseDto>>> GetByProject(int projectId)
        {
            var columns = await _columnService.GetByProjectAsync(projectId);
            return Ok(columns);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ColumnDetailsDto>> GetColumn(int id)
        {
            var column = await _columnService.GetByIdAsync(id);
            if (column == null) return NotFound();
            return Ok(column);
        }

        [HttpPost]
        public async Task<ActionResult<ColumnResponseDto>> CreateColumn(CreateColumnDto dto)
        {
            var column = await _columnService.CreateAsync(dto);
            if (column == null) return BadRequest("Project not found");

            return CreatedAtAction(nameof(GetColumn), new { id = column.Id }, column);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateColumn(int id, UpdateColumnDto dto)
        {
            var updated = await _columnService.UpdateAsync(id, dto);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColumn(int id)
        {
            var deleted = await _columnService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
