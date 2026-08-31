using System.ComponentModel.DataAnnotations;

namespace ToDoAPI.DTOs.Columns
{
    public class CreateColumnDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public int ProjectId { get; set; }

        public int Order { get; set; } = 0;
    }
}
