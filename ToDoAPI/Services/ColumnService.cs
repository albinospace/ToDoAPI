using Microsoft.EntityFrameworkCore;
using ToDoAPI.Data;
using ToDoAPI.DTOs.Columns;
using ToDoAPI.DTOs.ToDos;
using ToDoAPI.Models;

namespace ToDoAPI.Services
{
    public class ColumnService : IColumnService
    {
        private readonly AppDbContext _context;
        public ColumnService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ColumnResponseDto>> GetByProjectAsync(int projectId)
        {
            return await _context.Columns
                .Where(c => c.ProjectId == projectId)
                .OrderBy(c => c.Order)
                .Select(c => new ColumnResponseDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Order = c.Order,
                    ProjectId = c.ProjectId,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<ColumnDetailsDto?> GetByIdAsync(int id)
        {
            var column = await _context.Columns
                .Include(c => c.ToDoItems.OrderBy(t => t.Order))
                .FirstOrDefaultAsync(c => c.Id == id);

            if (column == null) return null;

            return new ColumnDetailsDto
            {
                Id = column.Id,
                Title = column.Title,
                Order = column.Order,
                ProjectId = column.ProjectId,
                CreatedAt = column.CreatedAt,
                Todos = column.ToDoItems.Select(t => new TodoResponseDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                    CreatedAt = t.CreatedAt,
                    CompletedAt = t.CompletedAt,
                    Order = t.Order,
                    ColumnId = t.ColumnId
                }).ToList()
            };
        }

        public async Task<ColumnResponseDto?> CreateAsync(CreateColumnDto dto)
        {
            var projectExists = await _context.Projects.AnyAsync(p => p.Id == dto.ProjectId);
            if (!projectExists) return null;

            var column = new Column
            {
                Title = dto.Title,
                Order = dto.Order,
                ProjectId = dto.ProjectId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Columns.Add(column);
            await _context.SaveChangesAsync();

            return new ColumnResponseDto
            {
                Id = column.Id,
                Title = column.Title,
                Order = column.Order,
                ProjectId = column.ProjectId,
                CreatedAt = column.CreatedAt
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateColumnDto dto)
        {
            var column = await _context.Columns.FindAsync(id);
            if (column == null) return false;

            column.Title = dto.Title;
            column.Order = dto.Order;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var column = await _context.Columns.FindAsync(id);
            if (column == null) return false;

            _context.Columns.Remove(column);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
