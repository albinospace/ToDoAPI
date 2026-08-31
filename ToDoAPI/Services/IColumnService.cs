using ToDoAPI.DTOs.Columns;

namespace ToDoAPI.Services
{
    public interface IColumnService
    {
        Task<IEnumerable<ColumnResponseDto>> GetByProjectAsync(int projectId);
        Task<ColumnDetailsDto?> GetByIdAsync(int id);
        Task<ColumnResponseDto?> CreateAsync(CreateColumnDto dto);
        Task<bool> UpdateAsync(int id, UpdateColumnDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
