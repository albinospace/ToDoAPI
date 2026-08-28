using Microsoft.EntityFrameworkCore;
using ToDoAPI.Data;
using ToDoAPI.DTOs;
using ToDoAPI.Models;

namespace ToDoAPI.Services
{
    public class ToDoService : IToDoService
    {
        private readonly AppDbContext _context;

        public ToDoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ToDoItem>> GetAllAsync(bool? completed = null)
        {
            var query = _context.ToDoItems.AsQueryable();

            if (completed.HasValue)
                query = query.Where(t => t.IsCompleted == completed.Value);

            return await query
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<ToDoItem?> GetByIdAsync(int id)
        {
            return await _context.ToDoItems.FindAsync(id);
        }

        public async Task<ToDoItem> CreateAsync(CreateToDoDto dto)
        {
            var todo = new ToDoItem
            {
                Title = dto.Title,
                Description = dto.Description,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.ToDoItems.Add(todo);
            await _context.SaveChangesAsync();

            return todo;
        }

        public async Task<bool> UpdateAsync(int id, UpdateToDoDto dto)
        {
            var todo = await _context.ToDoItems.FindAsync(id);
            if (todo == null) return false;

            todo.Title = dto.Title;
            todo.Description = dto.Description;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CompleteAsync(int id)
        {
            var todo = await _context.ToDoItems.FindAsync(id);
            if (todo == null) return false;

            todo.IsCompleted = true;
            todo.CompletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var todo = await _context.ToDoItems.FindAsync(id);
            if (todo == null) return false;

            _context.ToDoItems.Remove(todo);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
