using System.ComponentModel.DataAnnotations;
using ToDoAPI.Models;

namespace ToDoAPI.DTOs.ToDos
{
    public class CreateToDoDto
    {
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(200, ErrorMessage = "Title cannot be longer than 200 characters")]
        public string Title { get; set; }
        public string? Description { get; set; }

        [Required(ErrorMessage = "ColumnId is required")]
        public int ColumnId { get; set; }

        public int Order { get; set; } = 0;
        public DateTime? DueDate { get; set; }

        public Priority Priority { get; set; } = Priority.Medium;
    }
}
