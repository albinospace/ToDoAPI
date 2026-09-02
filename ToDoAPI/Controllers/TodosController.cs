using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoAPI.DTOs;
using ToDoAPI.DTOs.ToDos;
using ToDoAPI.Services;

namespace ToDoAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly IToDoService _todoService;
        public TodosController(IToDoService todoService)
        {
            _todoService = todoService;
        }

        private int GetUserId() =>
                int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        /// <summary>
        /// Get ToDo List for authorized user
        /// </summary>
        /// 
        // GET: api/todos
        // You can pass ?completed=true or ?completed=false
        [HttpGet]
        public async Task<ActionResult<PagedResult<TodoResponseDto>>> GetTodos(
            [FromQuery] bool? completed = null,
            [FromQuery] int? columnId = null,
            [FromQuery] int? projectId = null,
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10)
        {
            var todos = await _todoService.GetAllAsync(GetUserId(), completed, columnId, projectId, page, pageSize);
            return Ok(todos);
        }

        /// <summary>
        /// Get ToDoList item by id for authorized user.
        /// </summary>
        // GET: api/todos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoResponseDto>> GetTodo(int id)
        {
            var todo = await _todoService.GetByIdAsync(id, GetUserId());
            if (todo == null) return NotFound();

            return Ok(todo);
        }

        /// <summary>
        /// Create ToDoList item for authorized user.
        /// </summary>
        // POST: api/todos
        [HttpPost]
        public async Task<ActionResult<TodoResponseDto>> CreateTodo(CreateToDoDto dto)
        {
            var todo = await _todoService.CreateAsync(dto, GetUserId());
            if (todo == null)
                return BadRequest("Column not found");

            return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
        }

        /// <summary>
        /// Update ToDoList item with id for authorized user.
        /// </summary>
        // PUT: api/todos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(int id, UpdateToDoDto dto)
        {
            var updated = await _todoService.UpdateAsync(id, dto, GetUserId());
            if (!updated) return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Delete ToDoList item by id for authorized user.
        /// </summary>
        // DELETE: api/todos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var deleted = await _todoService.DeleteAsync(id, GetUserId());
            if (!deleted) return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Complete ToDoList item by id for authorized user.
        /// </summary>
        // COMPLETE: api/todos/5/complete
        [HttpPatch("{id}/complete")]
        public async Task<IActionResult> CompleteTodo(int id)
        {
            var completed = await _todoService.CompleteAsync(id, GetUserId());
            if (!completed) return NotFound();

            return NoContent();
        }
    }
}
