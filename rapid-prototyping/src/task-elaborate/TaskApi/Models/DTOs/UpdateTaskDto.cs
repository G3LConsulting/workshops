using System.ComponentModel.DataAnnotations;

namespace TaskApi.Models.DTOs;

/// <summary>
/// Data transfer object for updating an existing task
/// </summary>
public class UpdateTaskDto
{
    /// <summary>
    /// Title of the task
    /// </summary>
    /// <example>Complete project documentation - Updated</example>
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public string? Title { get; set; }

    /// <summary>
    /// Detailed description of the task
    /// </summary>
    /// <example>Updated: Write comprehensive API documentation including all endpoints and examples</example>
    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    public string? Description { get; set; }

    /// <summary>
    /// Due date for the task
    /// </summary>
    /// <example>2025-12-31T23:59:59Z</example>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Updated status of the task
    /// </summary>
    /// <example>InProgress</example>
    public TaskStatus? Status { get; set; }
}
