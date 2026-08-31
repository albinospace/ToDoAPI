using ToDoAPI.DTOs.Projects;
using ToDoAPI.Models;

namespace ToDoAPI.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectResponseDto>> GetAllAsync();
        Task<ProjectDetailsDto?> GetByIdAsync(int id);
        Task<ProjectResponseDto> CreateAsync(CreateProjectDto dto);
        Task<bool> UpdateAsync(int id, UpdateProjectDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
