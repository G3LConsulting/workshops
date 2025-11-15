using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskApi.Controllers;
using TaskApi.Data.Repositories;
using TaskApi.Models.DTOs;
using TaskApi.Models.Entities;
using TaskApi.Tests.Fixtures;
using Xunit;

namespace TaskApi.Tests.Controllers
{
    /// <summary>
    /// Unit tests for TasksController API endpoints
    /// </summary>
    public class TasksControllerTests
    {
        /// <summary>
        /// Tests for POST /api/tasks (Create endpoint)
        /// </summary>
        public class CreateTests
        {
            [Fact]
            public async Task Create_WithValidDto_Returns201Created()
            {
                // Arrange
                var mockRepo = new Mock<ITaskRepository>();
                var testTask = TestDataFixtures.SampleTask();
                mockRepo.Setup(r => r.CreateAsync(It.IsAny<TaskEntity>()))
                    .ReturnsAsync(testTask);
                
                var controller = new TasksController(mockRepo.Object);
                var dto = TestDataFixtures.SampleCreateDto();

                // Act
                var result = await controller.Create(dto);

                // Assert
                result.Result.Should().BeOfType<CreatedAtActionResult>();
                var createdResult = result.Result as CreatedAtActionResult;
                createdResult!.StatusCode.Should().Be(201);
            }

            [Fact]
            public async Task Create_WithValidDto_ReturnsTaskWithGeneratedId()
            {
                // Arrange
                var mockRepo = new Mock<ITaskRepository>();
                var testTask = TestDataFixtures.SampleTask();
                mockRepo.Setup(r => r.CreateAsync(It.IsAny<TaskEntity>()))
                    .ReturnsAsync(testTask);
                
                var controller = new TasksController(mockRepo.Object);
                var dto = TestDataFixtures.SampleCreateDto();

                // Act
                var result = await controller.Create(dto);

                // Assert
                var createdResult = result.Result as CreatedAtActionResult;
                var returnedDto = createdResult!.Value as TaskDto;
                returnedDto.Should().NotBeNull();
                returnedDto!.Id.Should().NotBeEmpty();
                returnedDto.Title.Should().Be(testTask.Title);
            }

            [Fact]
            public async Task Create_WithValidDto_CallsRepositoryCreateAsync()
            {
                // Arrange
                var mockRepo = new Mock<ITaskRepository>();
                var testTask = TestDataFixtures.SampleTask();
                mockRepo.Setup(r => r.CreateAsync(It.IsAny<TaskEntity>()))
                    .ReturnsAsync(testTask);
                
                var controller = new TasksController(mockRepo.Object);
                var dto = TestDataFixtures.SampleCreateDto();

                // Act
                await controller.Create(dto);

                // Assert
                mockRepo.Verify(r => r.CreateAsync(It.Is<TaskEntity>(t => 
                    t.Title == dto.Title && 
                    t.Description == dto.Description &&
                    t.IsComplete == false
                )), Times.Once);
            }

            // User Story 2: Validation Error Tests
            [Fact]
            public async Task Create_WithWhitespaceTitle_Returns400BadRequest()
            {
                // Arrange
                var mockRepo = new Mock<ITaskRepository>();
                var controller = new TasksController(mockRepo.Object);
                var dto = new CreateTaskDto 
                { 
                    Title = "   ", // Whitespace-only title
                    Description = "Valid description"
                };

                // Act
                var result = await controller.Create(dto);

                // Assert
                result.Result.Should().BeOfType<BadRequestObjectResult>();
                var badRequestResult = result.Result as BadRequestObjectResult;
                badRequestResult!.StatusCode.Should().Be(400);
            }

            [Fact]
            public async Task Create_WithWhitespaceTitle_ReturnsValidationErrors()
            {
                // Arrange
                var mockRepo = new Mock<ITaskRepository>();
                var controller = new TasksController(mockRepo.Object);
                var dto = new CreateTaskDto 
                { 
                    Title = "   ",
                    Description = "Valid description"
                };

                // Act
                var result = await controller.Create(dto);

                // Assert
                var badRequestResult = result.Result as BadRequestObjectResult;
                badRequestResult.Should().NotBeNull();
                badRequestResult!.Value.Should().NotBeNull();
            }
        }

        /// <summary>
        /// Tests for GET /api/tasks (GetAll endpoint)
        /// </summary>
        public class GetAllTests
        {
            [Fact]
            public async Task GetAll_WithNoFilters_Returns200OK()
            {
                // Arrange
                var tasks = new List<TaskEntity>
                {
                    TestDataFixtures.SampleTask(),
                    TestDataFixtures.SampleTask(),
                    TestDataFixtures.SampleTask()
                };
                var mockRepo = new Mock<ITaskRepository>();
                mockRepo.Setup(r => r.GetAllAsync(null, null))
                    .ReturnsAsync(tasks);
                
                var controller = new TasksController(mockRepo.Object);

                // Act
                var result = await controller.GetAll();

                // Assert
                result.Result.Should().BeOfType<OkObjectResult>();
                var okResult = result.Result as OkObjectResult;
                okResult!.StatusCode.Should().Be(200);
            }

            [Fact]
            public async Task GetAll_WithNoFilters_ReturnsAllTasks()
            {
                // Arrange
                var tasks = new List<TaskEntity>
                {
                    TestDataFixtures.SampleTask(t => t.Title = "Task 1"),
                    TestDataFixtures.SampleTask(t => t.Title = "Task 2"),
                    TestDataFixtures.SampleTask(t => t.Title = "Task 3")
                };
                var mockRepo = new Mock<ITaskRepository>();
                mockRepo.Setup(r => r.GetAllAsync(null, null))
                    .ReturnsAsync(tasks);
                
                var controller = new TasksController(mockRepo.Object);

                // Act
                var result = await controller.GetAll();

                // Assert
                var okResult = result.Result as OkObjectResult;
                var dtos = okResult!.Value as IEnumerable<TaskDto>;
                dtos.Should().HaveCount(3);
            }

            [Fact]
            public async Task GetAll_WithNoFilters_MapsEntitiesToDtos()
            {
                // Arrange
                var tasks = new List<TaskEntity>
                {
                    TestDataFixtures.SampleTask(t => { 
                        t.Title = "Task 1"; 
                        t.Description = "Description 1"; 
                    })
                };
                var mockRepo = new Mock<ITaskRepository>();
                mockRepo.Setup(r => r.GetAllAsync(null, null))
                    .ReturnsAsync(tasks);
                
                var controller = new TasksController(mockRepo.Object);

                // Act
                var result = await controller.GetAll();

                // Assert
                var okResult = result.Result as OkObjectResult;
                var dtos = (okResult!.Value as IEnumerable<TaskDto>)!.ToList();
                dtos[0].Title.Should().Be("Task 1");
                dtos[0].Description.Should().Be("Description 1");
                dtos[0].Id.Should().NotBeEmpty();
            }

            // User Story 4: Query Filtering Tests
            [Fact]
            public async Task GetAll_WithStatusFilter_ReturnsOnlyMatchingStatus()
            {
                // Arrange
                var tasks = new List<TaskEntity>
                {
                    TestDataFixtures.SampleTask(t => t.IsComplete = true),
                    TestDataFixtures.SampleTask(t => t.IsComplete = true)
                };
                var mockRepo = new Mock<ITaskRepository>();
                mockRepo.Setup(r => r.GetAllAsync("complete", null))
                    .ReturnsAsync(tasks);
                
                var controller = new TasksController(mockRepo.Object);

                // Act
                var result = await controller.GetAll(status: "complete");

                // Assert
                var okResult = result.Result as OkObjectResult;
                var dtos = (okResult!.Value as IEnumerable<TaskDto>)!.ToList();
                dtos.Should().HaveCount(2);
                dtos.Should().OnlyContain(dto => dto.IsComplete);
            }

            [Fact]
            public async Task GetAll_WithSearchFilter_ReturnsOnlyMatchingSearch()
            {
                // Arrange
                var tasks = new List<TaskEntity>
                {
                    TestDataFixtures.SampleTask(t => t.Title = "Important task"),
                    TestDataFixtures.SampleTask(t => t.Title = "Another important item")
                };
                var mockRepo = new Mock<ITaskRepository>();
                mockRepo.Setup(r => r.GetAllAsync(null, "important"))
                    .ReturnsAsync(tasks);
                
                var controller = new TasksController(mockRepo.Object);

                // Act
                var result = await controller.GetAll(search: "important");

                // Assert
                var okResult = result.Result as OkObjectResult;
                var dtos = (okResult!.Value as IEnumerable<TaskDto>)!.ToList();
                dtos.Should().HaveCount(2);
            }

            [Fact]
            public async Task GetAll_WithBothFilters_ReturnsTasksMatchingBoth()
            {
                // Arrange
                var tasks = new List<TaskEntity>
                {
                    TestDataFixtures.SampleTask(t => {
                        t.Title = "Important completed task";
                        t.IsComplete = true;
                    })
                };
                var mockRepo = new Mock<ITaskRepository>();
                mockRepo.Setup(r => r.GetAllAsync("complete", "important"))
                    .ReturnsAsync(tasks);
                
                var controller = new TasksController(mockRepo.Object);

                // Act
                var result = await controller.GetAll(status: "complete", search: "important");

                // Assert
                var okResult = result.Result as OkObjectResult;
                var dtos = (okResult!.Value as IEnumerable<TaskDto>)!.ToList();
                dtos.Should().HaveCount(1);
                dtos[0].IsComplete.Should().BeTrue();
            }

            [Fact]
            public async Task GetAll_WithStatusFilter_CallsRepositoryWithCorrectParameters()
            {
                // Arrange
                var mockRepo = new Mock<ITaskRepository>();
                mockRepo.Setup(r => r.GetAllAsync(It.IsAny<string?>(), It.IsAny<string?>()))
                    .ReturnsAsync(new List<TaskEntity>());
                
                var controller = new TasksController(mockRepo.Object);

                // Act
                await controller.GetAll(status: "complete", search: "test");

                // Assert
                mockRepo.Verify(r => r.GetAllAsync("complete", "test"), Times.Once);
            }
        }

        /// <summary>
        /// Tests for GET /api/tasks/{id} (GetById endpoint)
        /// </summary>
        public class GetByIdTests
        {
            [Fact]
            public async Task GetById_WithValidId_Returns200OK()
            {
                // Arrange
                var testTask = TestDataFixtures.SampleTask();
                var mockRepo = new Mock<ITaskRepository>();
                mockRepo.Setup(r => r.GetByIdAsync(testTask.Id))
                    .ReturnsAsync(testTask);
                
                var controller = new TasksController(mockRepo.Object);

                // Act
                var result = await controller.GetById(testTask.Id);

                // Assert
                result.Result.Should().BeOfType<OkObjectResult>();
                var okResult = result.Result as OkObjectResult;
                okResult!.StatusCode.Should().Be(200);
            }

            [Fact]
            public async Task GetById_WithValidId_ReturnsCorrectTask()
            {
                // Arrange
                var testTask = TestDataFixtures.SampleTask(t => {
                    t.Title = "Specific Task";
                    t.Description = "Specific Description";
                });
                var mockRepo = new Mock<ITaskRepository>();
                mockRepo.Setup(r => r.GetByIdAsync(testTask.Id))
                    .ReturnsAsync(testTask);
                
                var controller = new TasksController(mockRepo.Object);

                // Act
                var result = await controller.GetById(testTask.Id);

                // Assert
                var okResult = result.Result as OkObjectResult;
                var dto = okResult!.Value as TaskDto;
                dto.Should().NotBeNull();
                dto!.Id.Should().Be(testTask.Id);
                dto.Title.Should().Be("Specific Task");
                dto.Description.Should().Be("Specific Description");
            }

            [Fact]
            public async Task GetById_WithValidId_MapsDtoCorrectly()
            {
                // Arrange
                var testTask = TestDataFixtures.SampleTask();
                var mockRepo = new Mock<ITaskRepository>();
                mockRepo.Setup(r => r.GetByIdAsync(testTask.Id))
                    .ReturnsAsync(testTask);
                
                var controller = new TasksController(mockRepo.Object);

                // Act
                var result = await controller.GetById(testTask.Id);

                // Assert
                var okResult = result.Result as OkObjectResult;
                var dto = okResult!.Value as TaskDto;
                dto!.CreatedAt.Should().NotBe(default);
                dto.UpdatedAt.Should().NotBe(default);
                dto.IsComplete.Should().Be(testTask.IsComplete);
            }

            // User Story 2: 404 Error Tests
            [Fact]
            public async Task GetById_WithNonExistentId_Returns404NotFound()
            {
                // Arrange
                var mockRepo = new Mock<ITaskRepository>();
                mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync((TaskEntity?)null);
                
                var controller = new TasksController(mockRepo.Object);
                var nonExistentId = Guid.NewGuid();

                // Act
                var result = await controller.GetById(nonExistentId);

                // Assert
                result.Result.Should().BeOfType<NotFoundObjectResult>();
                var notFoundResult = result.Result as NotFoundObjectResult;
                notFoundResult!.StatusCode.Should().Be(404);
            }

            [Fact]
            public async Task GetById_WithNonExistentId_ReturnsErrorMessage()
            {
                // Arrange
                var mockRepo = new Mock<ITaskRepository>();
                mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync((TaskEntity?)null);
                
                var controller = new TasksController(mockRepo.Object);
                var nonExistentId = Guid.NewGuid();

                // Act
                var result = await controller.GetById(nonExistentId);

                // Assert
                var notFoundResult = result.Result as NotFoundObjectResult;
                notFoundResult.Should().NotBeNull();
                notFoundResult!.Value.Should().NotBeNull();
            }
        }

        /// <summary>
        /// Tests for PUT /api/tasks/{id} (Update endpoint)
        /// </summary>
        public class UpdateTests
        {
            [Fact]
            public async Task Update_WithValidData_Returns200OK()
            {
                // Arrange
                var existingTask = TestDataFixtures.SampleTask();
                var updatedTask = TestDataFixtures.SampleTask(t => {
                    t.Id = existingTask.Id;
                    t.Title = "Updated Title";
                });
                
                var mockRepo = new Mock<ITaskRepository>();
                mockRepo.Setup(r => r.GetByIdAsync(existingTask.Id))
                    .ReturnsAsync(existingTask);
                mockRepo.Setup(r => r.UpdateAsync(It.IsAny<TaskEntity>()))
                    .ReturnsAsync(updatedTask);
                
                var controller = new TasksController(mockRepo.Object);
                var dto = TestDataFixtures.SampleUpdateDto();

                // Act
                var result = await controller.Update(existingTask.Id, dto);

                // Assert
                result.Result.Should().BeOfType<OkObjectResult>();
                var okResult = result.Result as OkObjectResult;
                okResult!.StatusCode.Should().Be(200);
            }

            [Fact]
            public async Task Update_WithValidData_ReturnsUpdatedTask()
            {
                // Arrange
                var existingTask = TestDataFixtures.SampleTask();
                var updatedTask = TestDataFixtures.SampleTask(t => {
                    t.Id = existingTask.Id;
                    t.Title = "Updated Title";
                    t.Description = "Updated Description";
                });
                
                var mockRepo = new Mock<ITaskRepository>();
                mockRepo.Setup(r => r.GetByIdAsync(existingTask.Id))
                    .ReturnsAsync(existingTask);
                mockRepo.Setup(r => r.UpdateAsync(It.IsAny<TaskEntity>()))
                    .ReturnsAsync(updatedTask);
                
                var controller = new TasksController(mockRepo.Object);
                var dto = new UpdateTaskDto
                {
                    Title = "Updated Title",
                    Description = "Updated Description",
                    DueDate = DateTime.UtcNow.AddDays(10),
                    IsComplete = true
                };

                // Act
                var result = await controller.Update(existingTask.Id, dto);

                // Assert
                var okResult = result.Result as OkObjectResult;
                var returnedDto = okResult!.Value as TaskDto;
                returnedDto.Should().NotBeNull();
                returnedDto!.Title.Should().Be("Updated Title");
                returnedDto.Description.Should().Be("Updated Description");
            }

            [Fact]
            public async Task Update_WithValidData_CallsRepositoryUpdateAsync()
            {
                // Arrange
                var existingTask = TestDataFixtures.SampleTask();
                var mockRepo = new Mock<ITaskRepository>();
                mockRepo.Setup(r => r.GetByIdAsync(existingTask.Id))
                    .ReturnsAsync(existingTask);
                mockRepo.Setup(r => r.UpdateAsync(It.IsAny<TaskEntity>()))
                    .ReturnsAsync(existingTask);
                
                var controller = new TasksController(mockRepo.Object);
                var dto = TestDataFixtures.SampleUpdateDto();

                // Act
                await controller.Update(existingTask.Id, dto);

                // Assert
                mockRepo.Verify(r => r.UpdateAsync(It.Is<TaskEntity>(t => 
                    t.Id == existingTask.Id &&
                    t.Title == dto.Title
                )), Times.Once);
            }

            // User Story 2: Validation and 404 Error Tests
            [Fact]
            public async Task Update_WithWhitespaceTitle_Returns400BadRequest()
            {
                // Arrange
                var existingTask = TestDataFixtures.SampleTask();
                var mockRepo = new Mock<ITaskRepository>();
                mockRepo.Setup(r => r.GetByIdAsync(existingTask.Id))
                    .ReturnsAsync(existingTask);
                
                var controller = new TasksController(mockRepo.Object);
                var dto = new UpdateTaskDto
                {
                    Title = "   ", // Whitespace-only
                    Description = "Valid description",
                    DueDate = DateTime.UtcNow.AddDays(7),
                    IsComplete = false
                };

                // Act
                var result = await controller.Update(existingTask.Id, dto);

                // Assert
                result.Result.Should().BeOfType<BadRequestObjectResult>();
            }

            [Fact]
            public async Task Update_WithWhitespaceTitle_ReturnsValidationErrors()
            {
                // Arrange
                var existingTask = TestDataFixtures.SampleTask();
                var mockRepo = new Mock<ITaskRepository>();
                mockRepo.Setup(r => r.GetByIdAsync(existingTask.Id))
                    .ReturnsAsync(existingTask);
                
                var controller = new TasksController(mockRepo.Object);
                var dto = new UpdateTaskDto
                {
                    Title = "   ",
                    Description = "Valid description",
                    DueDate = DateTime.UtcNow.AddDays(7),
                    IsComplete = false
                };

                // Act
                var result = await controller.Update(existingTask.Id, dto);

                // Assert
                var badRequestResult = result.Result as BadRequestObjectResult;
                badRequestResult.Should().NotBeNull();
                badRequestResult!.Value.Should().NotBeNull();
            }

            [Fact]
            public async Task Update_WithNonExistentId_Returns404NotFound()
            {
                // Arrange
                var mockRepo = new Mock<ITaskRepository>();
                mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync((TaskEntity?)null);
                
                var controller = new TasksController(mockRepo.Object);
                var dto = TestDataFixtures.SampleUpdateDto();
                var nonExistentId = Guid.NewGuid();

                // Act
                var result = await controller.Update(nonExistentId, dto);

                // Assert
                result.Result.Should().BeOfType<NotFoundObjectResult>();
            }
        }

        /// <summary>
        /// Tests for DELETE /api/tasks/{id} (Delete endpoint)
        /// </summary>
        public class DeleteTests
        {
            [Fact]
            public async Task Delete_WithValidId_Returns204NoContent()
            {
                // Arrange
                var testTask = TestDataFixtures.SampleTask();
                var mockRepo = new Mock<ITaskRepository>();
                mockRepo.Setup(r => r.GetByIdAsync(testTask.Id))
                    .ReturnsAsync(testTask);
                mockRepo.Setup(r => r.DeleteAsync(testTask.Id))
                    .Returns(Task.CompletedTask);
                
                var controller = new TasksController(mockRepo.Object);

                // Act
                var result = await controller.Delete(testTask.Id);

                // Assert
                result.Should().BeOfType<NoContentResult>();
                var noContentResult = result as NoContentResult;
                noContentResult!.StatusCode.Should().Be(204);
            }

            [Fact]
            public async Task Delete_WithValidId_CallsRepositoryDeleteAsync()
            {
                // Arrange
                var testTask = TestDataFixtures.SampleTask();
                var mockRepo = new Mock<ITaskRepository>();
                mockRepo.Setup(r => r.GetByIdAsync(testTask.Id))
                    .ReturnsAsync(testTask);
                mockRepo.Setup(r => r.DeleteAsync(testTask.Id))
                    .Returns(Task.CompletedTask);
                
                var controller = new TasksController(mockRepo.Object);

                // Act
                await controller.Delete(testTask.Id);

                // Assert
                mockRepo.Verify(r => r.DeleteAsync(testTask.Id), Times.Once);
            }

            // User Story 2: 404 Error Test
            [Fact]
            public async Task Delete_WithNonExistentId_Returns404NotFound()
            {
                // Arrange
                var mockRepo = new Mock<ITaskRepository>();
                mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync((TaskEntity?)null);
                
                var controller = new TasksController(mockRepo.Object);
                var nonExistentId = Guid.NewGuid();

                // Act
                var result = await controller.Delete(nonExistentId);

                // Assert
                result.Should().BeOfType<NotFoundObjectResult>();
            }
        }

        /// <summary>
        /// Tests for PATCH /api/tasks/{id}/complete (MarkComplete endpoint)
        /// </summary>
        public class MarkCompleteTests
        {
            [Fact]
            public async Task MarkComplete_WithValidId_Returns200OK()
            {
                // Arrange
                var testTask = TestDataFixtures.SampleTask(t => t.IsComplete = false);
                var completedTask = TestDataFixtures.SampleTask(t => {
                    t.Id = testTask.Id;
                    t.IsComplete = true;
                });
                
                var mockRepo = new Mock<ITaskRepository>();
                mockRepo.Setup(r => r.GetByIdAsync(testTask.Id))
                    .ReturnsAsync(testTask);
                mockRepo.Setup(r => r.UpdateAsync(It.IsAny<TaskEntity>()))
                    .ReturnsAsync(completedTask);
                
                var controller = new TasksController(mockRepo.Object);

                // Act
                var result = await controller.MarkComplete(testTask.Id);

                // Assert
                result.Result.Should().BeOfType<OkObjectResult>();
                var okResult = result.Result as OkObjectResult;
                okResult!.StatusCode.Should().Be(200);
            }

            [Fact]
            public async Task MarkComplete_WithValidId_ReturnsTaskMarkedComplete()
            {
                // Arrange
                var testTask = TestDataFixtures.SampleTask(t => t.IsComplete = false);
                var completedTask = TestDataFixtures.SampleTask(t => {
                    t.Id = testTask.Id;
                    t.IsComplete = true;
                });
                
                var mockRepo = new Mock<ITaskRepository>();
                mockRepo.Setup(r => r.GetByIdAsync(testTask.Id))
                    .ReturnsAsync(testTask);
                mockRepo.Setup(r => r.UpdateAsync(It.IsAny<TaskEntity>()))
                    .ReturnsAsync(completedTask);
                
                var controller = new TasksController(mockRepo.Object);

                // Act
                var result = await controller.MarkComplete(testTask.Id);

                // Assert
                var okResult = result.Result as OkObjectResult;
                var dto = okResult!.Value as TaskDto;
                dto.Should().NotBeNull();
                dto!.IsComplete.Should().BeTrue();
            }

            [Fact]
            public async Task MarkComplete_WithNonExistentId_Returns404NotFound()
            {
                // Arrange
                var mockRepo = new Mock<ITaskRepository>();
                mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync((TaskEntity?)null);
                
                var controller = new TasksController(mockRepo.Object);
                var nonExistentId = Guid.NewGuid();

                // Act
                var result = await controller.MarkComplete(nonExistentId);

                // Assert
                result.Result.Should().BeOfType<NotFoundObjectResult>();
            }
        }

        /// <summary>
        /// Tests for PATCH /api/tasks/{id}/incomplete (MarkIncomplete endpoint)
        /// </summary>
        public class MarkIncompleteTests
        {
            [Fact]
            public async Task MarkIncomplete_WithValidId_Returns200OK()
            {
                // Arrange
                var testTask = TestDataFixtures.SampleTask(t => t.IsComplete = true);
                var incompleteTask = TestDataFixtures.SampleTask(t => {
                    t.Id = testTask.Id;
                    t.IsComplete = false;
                });
                
                var mockRepo = new Mock<ITaskRepository>();
                mockRepo.Setup(r => r.GetByIdAsync(testTask.Id))
                    .ReturnsAsync(testTask);
                mockRepo.Setup(r => r.UpdateAsync(It.IsAny<TaskEntity>()))
                    .ReturnsAsync(incompleteTask);
                
                var controller = new TasksController(mockRepo.Object);

                // Act
                var result = await controller.MarkIncomplete(testTask.Id);

                // Assert
                result.Result.Should().BeOfType<OkObjectResult>();
                var okResult = result.Result as OkObjectResult;
                okResult!.StatusCode.Should().Be(200);
            }

            [Fact]
            public async Task MarkIncomplete_WithValidId_ReturnsTaskMarkedIncomplete()
            {
                // Arrange
                var testTask = TestDataFixtures.SampleTask(t => t.IsComplete = true);
                var incompleteTask = TestDataFixtures.SampleTask(t => {
                    t.Id = testTask.Id;
                    t.IsComplete = false;
                });
                
                var mockRepo = new Mock<ITaskRepository>();
                mockRepo.Setup(r => r.GetByIdAsync(testTask.Id))
                    .ReturnsAsync(testTask);
                mockRepo.Setup(r => r.UpdateAsync(It.IsAny<TaskEntity>()))
                    .ReturnsAsync(incompleteTask);
                
                var controller = new TasksController(mockRepo.Object);

                // Act
                var result = await controller.MarkIncomplete(testTask.Id);

                // Assert
                var okResult = result.Result as OkObjectResult;
                var dto = okResult!.Value as TaskDto;
                dto.Should().NotBeNull();
                dto!.IsComplete.Should().BeFalse();
            }
        }
    }
}
