using Microsoft.EntityFrameworkCore;
using ToDoAPI.Data;
using ToDoAPI.DTOs.Columns;
using ToDoAPI.DTOs.Projects;
using ToDoAPI.DTOs.ToDos;
using ToDoAPI.Models;

namespace ToDoAPI.Services
{
    public class ProjectService : IProjectService
    {
        private readonly AppDbContext _context;

        public ProjectService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProjectResponseDto>> GetAllAsync(int userId)
        {
            return await _context.Projects
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new ProjectResponseDto
                {
                    Id = p.Id,
                    Name = p.Title,
                    Description = p.Description,
                    CreatedAt = p.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<ProjectDetailsDto?> GetByIdAsync(int id, int userId)
        {
            var project = await _context.Projects
                        .Include(p => p.Columns.OrderBy(c => c.Order))
                            .ThenInclude(c => c.ToDoItems.OrderBy(t => t.Order))
                        .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (project == null) return null;

            return new ProjectDetailsDto
            {
                Id = project.Id,
                Name = project.Title,
                Description = project.Description,
                CreatedAt = project.CreatedAt,
                Columns = project.Columns.Select(c => new ColumnDetailsDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Order = c.Order,
                    ProjectId = c.ProjectId,
                    CreatedAt = c.CreatedAt,
                    Todos = c.ToDoItems.Select(t => new TodoResponseDto
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
                }).ToList()
            };
        }

        public async Task<ProjectResponseDto> CreateAsync(CreateProjectDto dto, int userId)
        {
            var project = new Project
            {
                Title = dto.Title,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow,
                UserId = userId
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return new ProjectResponseDto
            {
                Id = project.Id,
                Name = project.Title,
                Description = project.Description,
                CreatedAt = project.CreatedAt
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateProjectDto dto, int userId)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);
            if (project == null) return false;

            project.Title = dto.Title;
            project.Description = dto.Description;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);
            if (project == null) return false;

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
