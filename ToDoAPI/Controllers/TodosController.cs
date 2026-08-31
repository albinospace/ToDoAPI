using Microsoft.AspNetCore.Mvc;
using ToDoAPI.DTOs;
using ToDoAPI.DTOs.ToDos;
using ToDoAPI.Services;

namespace ToDoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly IToDoService _todoService;
        public TodosController(IToDoService todoService)
        {
            _todoService = todoService;
        }

        // GET: api/todos
        // Можно передать ?completed=true или ?completed=false
        [HttpGet]
        public async Task<ActionResult<PagedResult<TodoResponseDto>>> GetTodos(
            [FromQuery] bool? completed = null,
            [FromQuery] int? columnId = null,
            [FromQuery] int? projectId = null,
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10)
        {
            var todos = await _todoService.GetAllAsync(completed, columnId, projectId, page, pageSize);
            return Ok(todos);
        }

        // GET: api/todos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoResponseDto>> GetTodo(int id)
        {
            var todo = await _todoService.GetByIdAsync(id);
            if (todo == null) return NotFound();

            return Ok(todo);
        }

        // POST: api/todos
        [HttpPost]
        public async Task<ActionResult<TodoResponseDto>> CreateTodo(CreateToDoDto dto)
        {
            var todo = await _todoService.CreateAsync(dto);
            if (todo == null)
                return BadRequest("Column not found");

            return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
        }

        // PUT: api/todos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(int id, UpdateToDoDto dto)
        {
            var updated = await _todoService.UpdateAsync(id, dto);
            if (!updated) return NotFound();

            return NoContent();
        }

        // DELETE: api/todos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var deleted = await _todoService.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }

        // COMPLETE: api/todos/5/complete
        [HttpPatch("{id}/complete")]
        public async Task<IActionResult> CompleteTodo(int id)
        {
            var completed = await _todoService.CompleteAsync(id);
            if (!completed) return NotFound();

            return NoContent();
        }
    }
}
