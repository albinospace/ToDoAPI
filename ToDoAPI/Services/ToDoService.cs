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
            int userId,
            bool? completed = null, 
            int? columnId = null, 
            int? projectId = null,
            int page = 1, 
            int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 50) pageSize = 50;

            var query = _context.ToDoItems
                .Include(t => t.Column)!
                .ThenInclude(c => c!.Project)
                .Where(t => t.Column!.Project!.UserId == userId)
                .AsQueryable();

            if (completed.HasValue)
                query = query.Where(t => t.IsCompleted == completed.Value);

            if (columnId.HasValue)
                query = query.Where(t => t.ColumnId == columnId.Value);

            if (projectId.HasValue)
                query = query.Where(t => t.Column!.ProjectId == projectId.Value);

            var totalCount = await query.CountAsync();

            var items = await query
                 .OrderBy(t => t.Order)
                 .ThenByDescending(t => t.CreatedAt)
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
                     Order = t.Order,
                     ColumnId = t.ColumnId,
                     DueDate = t.DueDate,
                     Priority = t.Priority
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

        public async Task<TodoResponseDto?> GetByIdAsync(int id, int userId)
        {
            var todo = await _context.ToDoItems
              .Include(t => t.Column)!
                  .ThenInclude(c => c!.Project)
              .FirstOrDefaultAsync(t => t.Id == id && t.Column!.Project!.UserId == userId);

            if (todo == null) return null;

            return new TodoResponseDto
            {
                Id = todo.Id,
                Title = todo.Title,
                Description = todo.Description,
                IsCompleted = todo.IsCompleted,
                CreatedAt = todo.CreatedAt,
                CompletedAt = todo.CompletedAt,
                Order = todo.Order,
                ColumnId = todo.ColumnId,
                DueDate = todo.DueDate,
                Priority = todo.Priority
            };
        }

        public async Task<TodoResponseDto?> CreateAsync(CreateToDoDto dto, int userId)
        {
            var column = await _context.Columns
                .Include(c => c.Project)
                .FirstOrDefaultAsync(c => c.Id == dto.ColumnId && c.Project!.UserId == userId);

            if (column == null) return null;

            var todo = new ToDoItem
            {
                Title = dto.Title,
                Description = dto.Description,
                ColumnId = dto.ColumnId,
                Order = dto.Order,
                DueDate = dto.DueDate,
                Priority = dto.Priority,
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
                ColumnId = todo.ColumnId,
                DueDate = todo.DueDate,
                Priority = todo.Priority
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateToDoDto dto, int userId)
        {
            var todo = await _context.ToDoItems
                        .Include(t => t.Column)!
                            .ThenInclude(c => c!.Project)
                        .FirstOrDefaultAsync(t => t.Id == id && t.Column!.Project!.UserId == userId);

            if (todo == null) return false;

            if (dto.Title is not null)
            {
                if (string.IsNullOrWhiteSpace(dto.Title))
                    return false;

                todo.Title = dto.Title;
            }

            if (dto.Description is not null)
                todo.Description = dto.Description;

            if (dto.ColumnId.HasValue)
            {
                var columnExists = await _context.Projects.AnyAsync(p => p.Id == dto.ColumnId.Value);
                if (!columnExists)
                    return false;

                todo.ColumnId = dto.ColumnId.Value;
            }

            if (dto.Order.HasValue)
                todo.Order = dto.Order.Value;

            if (dto.DueDate.HasValue)
                todo.DueDate = dto.DueDate;

            if (dto.Priority.HasValue)
                todo.Priority = dto.Priority.Value;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CompleteAsync(int id, int userId)
        {
            var todo = await _context.ToDoItems
                         .Include(t => t.Column)!
                             .ThenInclude(c => c!.Project)
                         .FirstOrDefaultAsync(t => t.Id == id && t.Column!.Project!.UserId == userId);

            if (todo == null) return false;

            todo.IsCompleted = true;
            todo.CompletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var todo = await _context.ToDoItems
                        .Include(t => t.Column)!
                            .ThenInclude(c => c!.Project)
                        .FirstOrDefaultAsync(t => t.Id == id && t.Column!.Project!.UserId == userId);

            if (todo == null) return false;

            _context.ToDoItems.Remove(todo);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
