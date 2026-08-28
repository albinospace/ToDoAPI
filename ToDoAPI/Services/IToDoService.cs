using ToDoAPI.DTOs;
using ToDoAPI.Models;

namespace ToDoAPI.Services
{
    public interface IToDoService
    {
        Task<PagedResult<ToDoItem>> GetAllAsync(bool? completed = null, int page = 1, int pageSize = 10);
        Task<ToDoItem?> GetByIdAsync(int id);
        Task<ToDoItem> CreateAsync(CreateToDoDto dto);
        Task<bool> UpdateAsync(int id, UpdateToDoDto dto);
        Task<bool> CompleteAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}
