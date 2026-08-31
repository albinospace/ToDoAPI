using System.ComponentModel.DataAnnotations;

namespace ToDoAPI.DTOs.Projects
{
    public class UpdateProjectDto
    {
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }
    }
}
