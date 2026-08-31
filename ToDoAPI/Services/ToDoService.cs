using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ToDoAPI.Data;
using ToDoAPI.DTOs;
using ToDoAPI.DTOs.ToDos;
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

        public async Task<PagedResult<TodoResponseDto>> GetAllAsync(
            bool? completed = null, 
            int? columnId = null, 
            int? projectId = null,
            int page = 1, 
            int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 50) pageSize = 50;

            var query = _context.ToDoItems.AsQueryable();

            if (completed.HasValue)
                query = query.Where(t => t.IsCompleted == completed.Value);

            if (columnId.HasValue)
                query = query.Where(t => t.ColumnId == columnId.Value);

            if (projectId.HasValue)
                query = query.Where(t => t.Column!.ProjectId == projectId.Value);

            var totalCount = await query.CountAsync();

            var items = await query
             .OrderByDescending(t => t.CreatedAt)
             .Skip((page - 1) * pageSize)
             .Take(pageSize)
             .Select(t => new TodoResponseDto
             {
                 Id = t.Id,
                 Title = t.Title,
                 Description = t.Description,
                 IsCompleted = t.IsCompleted,
                 CreatedAt = t.CreatedAt,
                 CompletedAt = t.CompletedAt,
                 ColumnId = t.ColumnId
             })
             .ToListAsync();

            return new PagedResult<TodoResponseDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<TodoResponseDto?> GetByIdAsync(int id)
        {
            var todo = await _context.ToDoItems.FindAsync(id);
            if (todo == null) return null;

            return new TodoResponseDto
            {
                Id = todo.Id,
                Title = todo.Title,
                Description = todo.Description,
                IsCompleted = todo.IsCompleted,
                CreatedAt = todo.CreatedAt,
                CompletedAt = todo.CompletedAt,
                ColumnId = todo.ColumnId
            };
        }

        public async Task<TodoResponseDto?> CreateAsync(CreateToDoDto dto)
        {
            var projectExists = await _context.Projects.AnyAsync(p => p.Id == dto.ColumnId);
            if (!projectExists)
                return null;

            var todo = new ToDoItem
            {
                Title = dto.Title,
                Description = dto.Description,
                ColumnId = dto.ColumnId,
                Order = dto.Order,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow,
            };

            _context.ToDoItems.Add(todo);
            await _context.SaveChangesAsync();

            return new TodoResponseDto
            {
                Id = todo.Id,
                Title = todo.Title,
                Description = todo.Description,
                IsCompleted = todo.IsCompleted,
                CreatedAt = todo.CreatedAt,
                CompletedAt = todo.CompletedAt,
                Order = todo.Order,
                ColumnId = todo.ColumnId
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateToDoDto dto)
        {
            var todo = await _context.ToDoItems.FindAsync(id);
            if (todo == null) return false;

            if (dto.ColumnId.HasValue)
            {
                var columnExists = await _context.Projects.AnyAsync(p => p.Id == dto.ColumnId.Value);
                if (!columnExists)
                    return false;

                todo.ColumnId = dto.ColumnId.Value;
            }

            if (dto.Order.HasValue)
                todo.Order = dto.Order.Value;

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
