using ToDoAPI.DTOs.Projects;

namespace ToDoAPI.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectResponseDto>> GetAllAsync(int userId);
        Task<ProjectDetailsDto?> GetByIdAsync(int id, int userId);
        Task<ProjectResponseDto> CreateAsync(CreateProjectDto dto, int userId);
        Task<bool> UpdateAsync(int id, UpdateProjectDto dto, int userId);
        Task<bool> DeleteAsync(int id, int userId);
    }
}
