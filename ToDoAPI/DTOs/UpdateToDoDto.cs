using System.ComponentModel.DataAnnotations;

namespace ToDoAPI.DTOs
{
    public class UpdateToDoDto
    {
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(200, ErrorMessage = "Title cannot be longer than 200 characters")]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
