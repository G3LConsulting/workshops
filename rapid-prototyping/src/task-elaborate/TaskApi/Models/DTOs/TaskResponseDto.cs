namespace TaskApi.Models.DTOs;

/// <summary>
/// Data transfer object for task responses
/// </summary>
public class TaskResponseDto
{
    /// <summary>
    /// Unique identifier for the task
    /// </summary>
    /// <example>1</example>
    public int Id { get; set; }

    /// <summary>
    /// Title of the task
    /// </summary>
    /// <example>Complete project documentation</example>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Detailed description of the task
    /// </summary>
    /// <example>Write comprehensive API documentation including all endpoints and examples</example>
    public string? Description { get; set; }

    /// <summary>
    /// Due date for the task
    /// </summary>
    /// <example>2025-12-31T23:59:59Z</example>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Date when the task was created
    /// </summary>
    /// <example>2025-11-14T10:30:00Z</example>
    public DateTime CreationDate { get; set; }

    /// <summary>
    /// Current status of the task
    /// </summary>
    /// <example>Pending</example>
    public TaskStatus Status { get; set; }
}
