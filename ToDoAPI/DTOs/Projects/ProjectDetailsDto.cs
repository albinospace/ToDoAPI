using ToDoAPI.DTOs.Columns;
using ToDoAPI.DTOs.ToDos;

namespace ToDoAPI.DTOs.Projects
{
    public class ProjectDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ColumnDetailsDto> Columns { get; set; } = new();
    }
}
