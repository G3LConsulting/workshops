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

        /// <summary>
        /// Get a task by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TaskDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TaskDto>> GetById(Guid id)
        {
            var task = await _repository.GetByIdAsync(id);
            if (task == null)
                return NotFound(new { detail = $"Task with id '{id}' was not found" });

            return Ok(MapToDto(task));
        }

        /// <summary>
        /// Update an existing task
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TaskDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TaskDto>> Update(Guid id, [FromBody] UpdateTaskDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                ModelState.AddModelError(nameof(dto.Title), "Title cannot be only whitespace");
                return BadRequest(ModelState);
            }

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { detail = $"Task with id '{id}' was not found" });

            existing.Title = dto.Title.Trim();
            existing.Description = dto.Description?.Trim();
            existing.DueDate = dto.DueDate;
            existing.IsComplete = dto.IsComplete;

            var updated = await _repository.UpdateAsync(existing);
            return Ok(MapToDto(updated));
        }

        /// <summary>
        /// Mark a task as complete
        /// </summary>
        [HttpPatch("{id}/complete")]
        [ProducesResponseType(typeof(TaskDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TaskDto>> MarkComplete(Guid id)
        {
            var task = await _repository.GetByIdAsync(id);
            if (task == null)
                return NotFound(new { detail = $"Task with id '{id}' was not found" });

            task.IsComplete = true;
            var updated = await _repository.UpdateAsync(task);
            return Ok(MapToDto(updated));
        }

        /// <summary>
        /// Mark a task as incomplete
        /// </summary>
        [HttpPatch("{id}/incomplete")]
        [ProducesResponseType(typeof(TaskDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TaskDto>> MarkIncomplete(Guid id)
        {
            var task = await _repository.GetByIdAsync(id);
            if (task == null)
                return NotFound(new { detail = $"Task with id '{id}' was not found" });

            task.IsComplete = false;
            var updated = await _repository.UpdateAsync(task);
            return Ok(MapToDto(updated));
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
    }
}
