using System.ComponentModel.DataAnnotations;

namespace TaskApi.Models;

/// <summary>
/// Represents a task in the task management system
/// </summary>
public class TaskItem
{
    /// <summary>
    /// Unique identifier for the task
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Title of the task
    /// </summary>
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Detailed description of the task
    /// </summary>
    [StringLength(1000)]
    public string? Description { get; set; }

    /// <summary>
    /// Due date for the task
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Date when the task was created
    /// </summary>
    public DateTime CreationDate { get; set; }

    /// <summary>
    /// Current status of the task
    /// </summary>
    [Required]
    public TaskStatus Status { get; set; } = TaskStatus.Pending;
}

/// <summary>
/// Enum representing the possible statuses of a task
/// </summary>
public enum TaskStatus
{
    Pending,
    InProgress,
    Completed
}
