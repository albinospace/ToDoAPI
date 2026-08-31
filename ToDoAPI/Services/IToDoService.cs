using ToDoAPI.DTOs;
using ToDoAPI.DTOs.ToDos;
using ToDoAPI.Models;

namespace ToDoAPI.Services
{
    public interface IToDoService
    {
        Task<PagedResult<TodoResponseDto>> GetAllAsync(
            bool? completed = null, 
            int? columnId = null, 
            int? projectId = null, 
            int page = 1,
            int pageSize = 10);

        Task<TodoResponseDto?> GetByIdAsync(int id);
        Task<TodoResponseDto?> CreateAsync(CreateToDoDto dto);
        Task<bool> UpdateAsync(int id, UpdateToDoDto dto);
        Task<bool> CompleteAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}
