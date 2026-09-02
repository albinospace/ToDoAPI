using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoAPI.DTOs.Projects;
using ToDoAPI.Services;

namespace ToDoAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        /// <summary>
        /// Get all projects for authorized user.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectResponseDto>>> GetProjects()
        {
            var projects = await _projectService.GetAllAsync(GetUserId());
            return Ok(projects);
        }

        /// <summary>
        /// Get project by id for authorized user.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDetailsDto>> GetProject(int id)
        {
            var project = await _projectService.GetByIdAsync(id, GetUserId());
            if (project == null) return NotFound();

            return Ok(project);
        }

        /// <summary>
        /// Create new project for authorized user.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ProjectResponseDto>> CreateProject(CreateProjectDto dto)
        {
            var project = await _projectService.CreateAsync(dto, GetUserId());
            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
        }

        /// <summary>
        /// Update project by id for authorized user.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, UpdateProjectDto dto)
        {
            var updated = await _projectService.UpdateAsync(id, dto, GetUserId());
            if (!updated) return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Delete project by id for authorized user.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var deleted = await _projectService.DeleteAsync(id, GetUserId());
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
