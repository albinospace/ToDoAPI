using ToDoAPI.DTOs.Columns;

namespace ToDoAPI.Services
{
    public interface IColumnService
    {
        Task<IEnumerable<ColumnResponseDto>> GetByProjectAsync(int projectId, int userId);
        Task<ColumnDetailsDto?> GetByIdAsync(int id, int userId);
        Task<ColumnResponseDto?> CreateAsync(CreateColumnDto dto, int userId);
        Task<bool> UpdateAsync(int id, UpdateColumnDto dto, int userId);
        Task<bool> DeleteAsync(int id, int userId);
    }
}
