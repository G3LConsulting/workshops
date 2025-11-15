using Moq;
using TaskApi.Data.Repositories;
using TaskApi.Models.Entities;

namespace TaskApi.Tests.Mocks
{
    /// <summary>
    /// Helper methods for creating configured mock repositories
    /// </summary>
    public static class MockTaskRepository
    {
        /// <summary>
        /// Creates a mock repository configured for GetById scenarios
        /// </summary>
        /// <param name="taskToReturn">The task entity to return, or null for 404 scenario</param>
        public static Mock<ITaskRepository> CreateMockForGetById(TaskEntity? taskToReturn)
        {
            var mockRepo = new Mock<ITaskRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(taskToReturn);
            return mockRepo;
        }

        /// <summary>
        /// Creates a mock repository configured for GetAll scenarios
        /// </summary>
        /// <param name="tasksToReturn">The list of tasks to return</param>
        public static Mock<ITaskRepository> CreateMockForGetAll(List<TaskEntity> tasksToReturn)
        {
            var mockRepo = new Mock<ITaskRepository>();
            mockRepo.Setup(r => r.GetAllAsync(It.IsAny<string?>(), It.IsAny<string?>()))
                .ReturnsAsync(tasksToReturn);
            return mockRepo;
        }
    }
}
