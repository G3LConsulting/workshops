using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskApi.Data;
using TaskApi.Models;
using TaskApi.Models.DTOs;

namespace TaskApi.Controllers;

/// <summary>
/// Controller for managing tasks
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TasksController : ControllerBase
{
    private readonly TaskDbContext _context;
    private readonly ILogger<TasksController> _logger;

    public TasksController(TaskDbContext context, ILogger<TasksController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves a paginated list of all tasks with optional filtering by status
    /// </summary>
    /// <param name="status">Optional filter by task status (Pending, InProgress, Completed)</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Number of items per page (default: 10, max: 100)</param>
    /// <returns>A paginated list of tasks</returns>
    /// <response code="200">Returns the paginated list of tasks</response>
    /// <response code="400">If the pagination parameters are invalid</response>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<TaskResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaginatedResponse<TaskResponseDto>>> GetTasks(
        [FromQuery] Models.TaskStatus? status = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            // Validate pagination parameters
            if (pageNumber < 1)
            {
                return BadRequest(new { error = "Page number must be greater than 0" });
            }

            if (pageSize < 1 || pageSize > 100)
            {
                return BadRequest(new { error = "Page size must be between 1 and 100" });
            }

            // Build query with optional filtering
            IQueryable<TaskItem> query = _context.Tasks;

            if (status.HasValue)
            {
                query = query.Where(t => t.Status == status.Value);
            }

            // Get total count for pagination
            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            // Apply pagination
            var tasks = await query
                .OrderByDescending(t => t.CreationDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Map to DTOs
            var taskDtos = tasks.Select(t => new TaskResponseDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                CreationDate = t.CreationDate,
                Status = t.Status
            });

            var response = new PaginatedResponse<TaskResponseDto>
            {
                Items = taskDtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                HasPreviousPage = pageNumber > 1,
                HasNextPage = pageNumber < totalPages
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tasks");
            return StatusCode(500, new { error = "An error occurred while retrieving tasks" });
        }
    }

    /// <summary>
    /// Retrieves a specific task by its ID
    /// </summary>
    /// <param name="id">The task ID</param>
    /// <returns>The requested task</returns>
    /// <response code="200">Returns the requested task</response>
    /// <response code="404">If the task is not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskResponseDto>> GetTask(int id)
    {
        try
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound(new { error = $"Task with ID {id} not found" });
            }

            var taskDto = new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                CreationDate = task.CreationDate,
                Status = task.Status
            };

            return Ok(taskDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving task with ID {TaskId}", id);
            return StatusCode(500, new { error = "An error occurred while retrieving the task" });
        }
    }

    /// <summary>
    /// Creates a new task
    /// </summary>
    /// <param name="createTaskDto">The task creation data</param>
    /// <returns>The newly created task</returns>
    /// <response code="201">Returns the newly created task</response>
    /// <response code="400">If the request data is invalid</response>
    [HttpPost]
    [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TaskResponseDto>> CreateTask([FromBody] CreateTaskDto createTaskDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = new TaskItem
            {
                Title = createTaskDto.Title,
                Description = createTaskDto.Description,
                DueDate = createTaskDto.DueDate,
                CreationDate = DateTime.UtcNow,
                Status = createTaskDto.Status ?? Models.TaskStatus.Pending
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            var taskDto = new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                CreationDate = task.CreationDate,
                Status = task.Status
            };

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, taskDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating task");
            return StatusCode(500, new { error = "An error occurred while creating the task" });
        }
    }

    /// <summary>
    /// Updates an existing task by its ID
    /// </summary>
    /// <param name="id">The task ID</param>
    /// <param name="updateTaskDto">The task update data</param>
    /// <returns>The updated task</returns>
    /// <response code="200">Returns the updated task</response>
    /// <response code="400">If the request data is invalid</response>
    /// <response code="404">If the task is not found</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskResponseDto>> UpdateTask(int id, [FromBody] UpdateTaskDto updateTaskDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound(new { error = $"Task with ID {id} not found" });
            }

            // Update only provided fields
            if (updateTaskDto.Title != null)
            {
                task.Title = updateTaskDto.Title;
            }

            if (updateTaskDto.Description != null)
            {
                task.Description = updateTaskDto.Description;
            }

            if (updateTaskDto.DueDate.HasValue)
            {
                task.DueDate = updateTaskDto.DueDate;
            }

            if (updateTaskDto.Status.HasValue)
            {
                task.Status = updateTaskDto.Status.Value;
            }

            await _context.SaveChangesAsync();

            var taskDto = new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                CreationDate = task.CreationDate,
                Status = task.Status
            };

            return Ok(taskDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating task with ID {TaskId}", id);
            return StatusCode(500, new { error = "An error occurred while updating the task" });
        }
    }

    /// <summary>
    /// Deletes a task by its ID
    /// </summary>
    /// <param name="id">The task ID</param>
    /// <returns>No content</returns>
    /// <response code="204">If the task was successfully deleted</response>
    /// <response code="404">If the task is not found</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTask(int id)
    {
        try
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound(new { error = $"Task with ID {id} not found" });
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting task with ID {TaskId}", id);
            return StatusCode(500, new { error = "An error occurred while deleting the task" });
        }
    }
}
