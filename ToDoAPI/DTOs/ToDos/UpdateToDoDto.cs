using System.ComponentModel.DataAnnotations;
using ToDoAPI.Models;

namespace ToDoAPI.DTOs.ToDos
{
    public class UpdateToDoDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? ColumnId { get; set; }
        public int? Order { get; set; }
        public DateTime? DueDate { get; set; }
        public Priority? Priority { get; set; }
    }
}
