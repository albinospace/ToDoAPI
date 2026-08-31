using System.ComponentModel.DataAnnotations;

namespace ToDoAPI.DTOs.Columns
{
    public class UpdateColumnDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        public int Order { get; set; }
    }
}
