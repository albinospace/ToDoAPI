namespace ToDoAPI.DTOs.Columns
{
    public class ColumnResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Order { get; set; }
        public int ProjectId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
