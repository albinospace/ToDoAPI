namespace ToDoAPI.Models
{
    public class Column
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Order { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int ProjectId { get; set; }
        public Project? Project { get; set; }
        public List<ToDoItem> ToDoItems { get; set; } = new();
    }
}
