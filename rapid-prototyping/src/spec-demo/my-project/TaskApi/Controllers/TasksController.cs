using Microsoft.AspNetCore.Mvc;
using TaskApi.Data.Repositories;
using TaskApi.Models.DTOs;
using TaskApi.Models.Entities;

namespace TaskApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _repository;

        public TasksController(ITaskRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Get all tasks with optional filtering
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TaskDto>), 200)]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetAll(
            [FromQuery] string? status = null, 
            [FromQuery] string? search = null)
        {
            var tasks = await _repository.GetAllAsync(status, search);
            return Ok(tasks.Select(MapToDto));
        }

        /// <summary>
        /// Create a new task
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(TaskDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<TaskDto>> Create([FromBody] CreateTaskDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                ModelState.AddModelError(nameof(dto.Title), "Title cannot be only whitespace");
                return BadRequest(ModelState);
            }

            var entity = new TaskEntity
            {
                Title = dto.Title.Trim(),
                Description = dto.Description?.Trim(),
                DueDate = dto.DueDate
            };

            var created = await _repository.CreateAsync(entity);
            var result = MapToDto(created);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        private static TaskDto MapToDto(TaskEntity entity) => new TaskDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            DueDate = entity.DueDate,
            IsComplete = entity.IsComplete,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };

        // Placeholder for GetById (will be implemented in User Story 2)
        private ActionResult<TaskDto> GetById(Guid id)
        {
            return NotFound();
        }
    }
}
