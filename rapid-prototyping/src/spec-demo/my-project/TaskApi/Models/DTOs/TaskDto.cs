namespace TaskApi.Models.DTOs
{
    /// <summary>
    /// Represents a task in API responses
    /// </summary>
    public class TaskDto
    {
        /// <summary>
        /// Unique identifier for the task
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Task title (1-100 characters)
        /// </summary>
        public string Title { get; set; } = string.Empty;
        
        /// <summary>
        /// Detailed task description (optional, max 500 characters)
        /// </summary>
        public string? Description { get; set; }
        
        /// <summary>
        /// Optional due date in UTC
        /// </summary>
        public DateTime? DueDate { get; set; }
        
        /// <summary>
        /// Whether the task is completed
        /// </summary>
        public bool IsComplete { get; set; }
        
        /// <summary>
        /// When the task was created (UTC)
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// When the task was last updated (UTC)
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
