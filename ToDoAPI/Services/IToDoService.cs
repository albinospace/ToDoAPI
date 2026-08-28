using ToDoAPI.DTOs;
using ToDoAPI.Models;

namespace ToDoAPI.Services
{
    public interface IToDoService
    {
        Task<IEnumerable<ToDoItem>> GetAllAsync(bool? completed = null);
        Task<ToDoItem?> GetByIdAsync(int id);
        Task<ToDoItem> CreateAsync(CreateToDoDto dto);
        Task<bool> UpdateAsync(int id, UpdateToDoDto dto);
        Task<bool> CompleteAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}
