using System.ComponentModel.DataAnnotations;

namespace ToDoAPI.DTOs
{
    public class CreateToDoDto
    {
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(200, ErrorMessage = "Title cannot be longer than 200 characters")]
        public string Title { get; set; }
        public string? Description { get; set; }
    }
}
