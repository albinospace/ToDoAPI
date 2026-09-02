using ToDoAPI.DTOs;
using ToDoAPI.DTOs.ToDos;
using ToDoAPI.Models;

namespace ToDoAPI.Services
{
    public interface IToDoService
    {
        Task<PagedResult<TodoResponseDto>> GetAllAsync(
            int userId,
            bool? completed = null, 
            int? columnId = null, 
            int? projectId = null, 
            int page = 1,
            int pageSize = 10);

        Task<TodoResponseDto?> GetByIdAsync(int id, int userId);
        Task<TodoResponseDto?> CreateAsync(CreateToDoDto dto, int userId);
        Task<bool> UpdateAsync(int id, UpdateToDoDto dto, int userId);
        Task<bool> CompleteAsync(int id, int userId);
        Task<bool> DeleteAsync(int id, int userId);
    }
}
