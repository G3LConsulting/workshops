using System.ComponentModel.DataAnnotations;

namespace TaskApi.Models.DTOs
{
    /// <summary>
    /// Input model for updating an existing task
    /// </summary>
    public class UpdateTaskDto
    {
        /// <summary>
        /// Task title (required, 1-100 characters)
        /// </summary>
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 100 characters")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Detailed task description (optional, max 500 characters)
        /// </summary>
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        /// <summary>
        /// Optional due date
        /// </summary>
        public DateTime? DueDate { get; set; }
        
        /// <summary>
        /// Task completion status
        /// </summary>
        public bool IsComplete { get; set; }
    }
}
