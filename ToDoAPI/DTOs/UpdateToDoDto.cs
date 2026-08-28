namespace ToDoAPI.DTOs
{
    public class UpdateToDoDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
