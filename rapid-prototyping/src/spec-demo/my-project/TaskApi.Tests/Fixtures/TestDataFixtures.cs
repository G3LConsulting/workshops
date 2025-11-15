using TaskApi.Models.DTOs;
using TaskApi.Models.Entities;

namespace TaskApi.Tests.Fixtures
{
    /// <summary>
    /// Provides reusable test data for unit tests
    /// </summary>
    public static class TestDataFixtures
    {
        /// <summary>
        /// Creates a sample TaskEntity with default values
        /// </summary>
        public static TaskEntity SampleTask()
        {
            return new TaskEntity
            {
                Id = Guid.NewGuid(),
                Title = "Sample Task",
                Description = "Sample task description",
                DueDate = DateTime.UtcNow.AddDays(7),
                IsComplete = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Creates a sample TaskEntity with customization callback
        /// </summary>
        /// <param name="customize">Action to customize the task</param>
        public static TaskEntity SampleTask(Action<TaskEntity> customize)
        {
            var task = SampleTask();
            customize(task);
            return task;
        }

        /// <summary>
        /// Creates a sample CreateTaskDto with default values
        /// </summary>
        public static CreateTaskDto SampleCreateDto()
        {
            return new CreateTaskDto
            {
                Title = "Test Task",
                Description = "Test task description",
                DueDate = DateTime.UtcNow.AddDays(7)
            };
        }

        /// <summary>
        /// Creates a sample UpdateTaskDto with default values
        /// </summary>
        public static UpdateTaskDto SampleUpdateDto()
        {
            return new UpdateTaskDto
            {
                Title = "Updated Task",
                Description = "Updated task description",
                DueDate = DateTime.UtcNow.AddDays(14),
                IsComplete = false
            };
        }
    }
}
