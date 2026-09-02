using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoAPI.DTOs.Columns;
using ToDoAPI.Services;

namespace ToDoAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ColumnsController : ControllerBase
    {
        private readonly IColumnService _columnService;

        public ColumnsController(IColumnService columnService)
        {
            _columnService = columnService;
        }

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        /// <summary>
        /// Get columns by project id for authorized user.
        /// </summary>
        [HttpGet("by-project/{projectId}")]
        public async Task<ActionResult<IEnumerable<ColumnResponseDto>>> GetByProject(int projectId)
        {
            var columns = await _columnService.GetByProjectAsync(projectId, GetUserId());
            return Ok(columns);
        }


        /// <summary>
        /// Get column with id for authorized user.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ColumnDetailsDto>> GetColumn(int id)
        {
            var column = await _columnService.GetByIdAsync(id, GetUserId());
            if (column == null) return NotFound();
            return Ok(column);
        }

        /// <summary>
        /// Create new column in project for authorized user.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ColumnResponseDto>> CreateColumn(CreateColumnDto dto)
        {
            var column = await _columnService.CreateAsync(dto, GetUserId());
            if (column == null) return BadRequest("Project not found");

            return CreatedAtAction(nameof(GetColumn), new { id = column.Id }, column);
        }


        /// <summary>
        /// Update column by id in project for authorized user.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateColumn(int id, UpdateColumnDto dto)
        {
            var updated = await _columnService.UpdateAsync(id, dto, GetUserId());
            if (!updated) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Delete column by id in project for authorized user.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColumn(int id)
        {
            var deleted = await _columnService.DeleteAsync(id, GetUserId());
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
