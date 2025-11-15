# Quickstart: Running and Writing Unit Tests

**Feature**: 002-unit-tests  
**Date**: 2025-11-15  
**Audience**: Developers working on TaskApi

---

## Prerequisites

- .NET 9.0 SDK installed
- TaskApi project building successfully
- Basic understanding of xUnit, Moq, and FluentAssertions

---

## Running Tests

### Run All Tests

```bash
# From repository root
dotnet test

# From test project directory
cd TaskApi.Tests
dotnet test
```

**Expected Output**:
```
Starting test execution, please wait...
A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:    21, Skipped:     0, Total:    21, Duration: < 2 s
```

### Run Specific Test Class

```bash
dotnet test --filter "FullyQualifiedName~TasksControllerTests.CreateTests"
```

### Run Tests with Coverage

```bash
# Generate coverage report
dotnet test --collect:"XPlat Code Coverage"

# View coverage (look for coverage.cobertura.xml in TestResults folder)
# Target: 80%+ coverage of TasksController
```

### Run Tests in Watch Mode

```bash
dotnet watch test
```

**Use case**: Automatically re-run tests when code changes during development.

---

## Project Structure

```
TaskApi.Tests/
├── TaskApi.Tests.csproj         # Test project file
├── Controllers/
│   └── TasksControllerTests.cs  # 21+ unit tests for all endpoints
├── Fixtures/
│   └── TestDataFixtures.cs      # Reusable test data builders
└── Mocks/
    └── MockTaskRepository.cs    # Helper for creating configured mocks
```

---

## Writing a New Test

### Step 1: Choose Test Location

Tests are organized by controller and nested by endpoint:

```csharp
public class TasksControllerTests
{
    public class GetAllTests { }      // Tests for GET /api/tasks
    public class CreateTests { }      // Tests for POST /api/tasks
    public class GetByIdTests { }     // Tests for GET /api/tasks/{id}
    public class UpdateTests { }      // Tests for PUT /api/tasks/{id}
    public class DeleteTests { }      // Tests for DELETE /api/tasks/{id}
    public class MarkCompleteTests { }    // Tests for PATCH /api/tasks/{id}/complete
    public class MarkIncompleteTests { }  // Tests for PATCH /api/tasks/{id}/incomplete
}
```

### Step 2: Follow AAA Pattern

All tests must follow **Arrange-Act-Assert** structure:

```csharp
[Fact]
public async Task MethodName_Scenario_ExpectedBehavior()
{
    // Arrange: Setup dependencies, test data, and controller
    var mockRepo = new Mock<ITaskRepository>();
    mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
        .ReturnsAsync(TestDataFixtures.SampleTask());
    var controller = new TasksController(mockRepo.Object);
    var testId = Guid.NewGuid();

    // Act: Call the method under test
    var result = await controller.GetById(testId);

    // Assert: Verify the outcome
    result.Should().BeOfType<ActionResult<TaskDto>>();
    var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
    var dto = okResult.Value.Should().BeOfType<TaskDto>().Subject;
    dto.Id.Should().Be(testId);
}
```

### Step 3: Use Test Naming Convention

**Format**: `MethodName_Scenario_ExpectedBehavior`

**Examples**:
- ✅ `Create_WithValidDto_Returns201Created`
- ✅ `GetById_WithNonExistentId_Returns404NotFound`
- ✅ `Update_WithWhitespaceTitle_Returns400BadRequest`
- ❌ `TestCreate` (too vague)
- ❌ `ShouldReturn404` (missing context)

---

## Common Testing Patterns

### Pattern 1: Mock Repository Returns Data

```csharp
// Arrange
var testTask = TestDataFixtures.SampleTask();
mockRepo.Setup(r => r.GetByIdAsync(testTask.Id))
    .ReturnsAsync(testTask);

// Act
var result = await controller.GetById(testTask.Id);

// Assert
result.Should().BeOfType<ActionResult<TaskDto>>();
var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
var dto = okResult.Value.Should().BeOfType<TaskDto>().Subject;
dto.Id.Should().Be(testTask.Id);
dto.Title.Should().Be(testTask.Title);
```

### Pattern 2: Mock Repository Returns Null (404 Scenario)

```csharp
// Arrange
mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
    .ReturnsAsync((TaskEntity?)null);

// Act
var result = await controller.GetById(Guid.NewGuid());

// Assert
result.Result.Should().BeOfType<NotFoundObjectResult>();
```

### Pattern 3: Test Validation Errors

```csharp
// Arrange
var dto = new CreateTaskDto { Title = "   " }; // Whitespace-only title

// Act
var result = await controller.Create(dto);

// Assert
result.Result.Should().BeOfType<BadRequestObjectResult>();
var badRequest = result.Result as BadRequestObjectResult;
badRequest.Value.Should().BeOfType<SerializableError>();
```

### Pattern 4: Test Collection Results

```csharp
// Arrange
var tasks = new List<TaskEntity>
{
    TestDataFixtures.SampleTask(t => t.IsComplete = true),
    TestDataFixtures.SampleTask(t => t.IsComplete = true),
    TestDataFixtures.SampleTask(t => t.IsComplete = false)
};
mockRepo.Setup(r => r.GetAllAsync("complete", null))
    .ReturnsAsync(tasks.Where(t => t.IsComplete).ToList());

// Act
var result = await controller.GetAll(status: "complete");

// Assert
var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
var dtos = okResult.Value.Should().BeAssignableTo<IEnumerable<TaskDto>>().Subject;
dtos.Should().HaveCount(2);
dtos.Should().OnlyContain(dto => dto.IsComplete);
```

### Pattern 5: Verify Repository Method Called

```csharp
// Arrange
var dto = new CreateTaskDto { Title = "Test Task" };
mockRepo.Setup(r => r.CreateAsync(It.IsAny<TaskEntity>()))
    .ReturnsAsync(TestDataFixtures.SampleTask());

// Act
await controller.Create(dto);

// Assert
mockRepo.Verify(r => r.CreateAsync(It.Is<TaskEntity>(t => 
    t.Title == "Test Task" && 
    t.IsComplete == false
)), Times.Once);
```

---

## Test Data Fixtures

Use `TestDataFixtures` for consistent test data:

```csharp
// Simple sample task
var task = TestDataFixtures.SampleTask();

// Customized task
var completeTask = TestDataFixtures.SampleTask(t => {
    t.IsComplete = true;
    t.Title = "Custom Title";
});

// Sample DTO
var dto = TestDataFixtures.SampleCreateDto();
```

**Why use fixtures?**
- Consistent test data across all tests
- Reduces boilerplate in arrange sections
- Easy to update if entity structure changes
- Customization support for specific scenarios

---

## FluentAssertions Tips

### Asserting on ActionResult<T>

```csharp
// Option 1: Two-step unwrap
var actionResult = await controller.GetById(id);
var okResult = actionResult.Result.Should().BeOfType<OkObjectResult>().Subject;
var dto = okResult.Value.Should().BeOfType<TaskDto>().Subject;

// Option 2: Direct value access (when you know the result type)
var result = await controller.GetById(id);
((OkObjectResult)result.Result).Value.Should().BeOfType<TaskDto>();
```

### Asserting on Collections

```csharp
// Count
dtos.Should().HaveCount(3);

// Contents
dtos.Should().Contain(dto => dto.Title == "Expected");
dtos.Should().OnlyContain(dto => dto.IsComplete == true);

// Ordering
dtos.Should().BeInAscendingOrder(dto => dto.CreatedAt);
```

### Asserting on Complex Objects

```csharp
// Property-by-property
dto.Id.Should().Be(expectedId);
dto.Title.Should().Be("Expected Title");
dto.IsComplete.Should().BeFalse();

// Object equality (if Equals implemented)
dto.Should().BeEquivalentTo(expectedDto);

// Exclude properties
dto.Should().BeEquivalentTo(expectedDto, options => options.Excluding(d => d.Id));
```

---

## Debugging Tests

### Run Single Test in Debug Mode

**VS Code**:
1. Open test file
2. Click debug icon above test method
3. Set breakpoints as needed

**Command Line**:
```bash
# Run with verbose output
dotnet test --logger "console;verbosity=detailed"
```

### Common Issues

**Issue**: Test fails with "Object reference not set to an instance"
**Solution**: Mock not configured for the scenario
```csharp
// Add missing mock setup
mockRepo.Setup(r => r.SomeMethod(It.IsAny<Guid>()))
    .ReturnsAsync(someValue);
```

**Issue**: Test fails with "Expected OkObjectResult but found ActionResult"
**Solution**: Access `.Result` property of `ActionResult<T>`
```csharp
// Incorrect
var okResult = result.Should().BeOfType<OkObjectResult>(); // ❌

// Correct
var okResult = result.Result.Should().BeOfType<OkObjectResult>(); // ✅
```

**Issue**: Async test hangs forever
**Solution**: Always `await` async calls, never use `.Result` or `.Wait()`
```csharp
// Incorrect
var result = controller.GetById(id).Result; // ❌ Can deadlock

// Correct
var result = await controller.GetById(id); // ✅
```

---

## Code Coverage Guidelines

**Target**: 80%+ line coverage of `TasksController.cs`

### What to Cover

✅ **Must Cover**:
- All endpoint success paths (200, 201, 204)
- All validation failure paths (400)
- All not-found paths (404)
- DTO mapping (entity → DTO)
- Query parameter filtering

✅ **Should Cover**:
- Edge cases (empty strings, null values, whitespace)
- Boundary conditions (max length strings)

❌ **Don't Need to Cover**:
- Repository implementations (not unit testing scope)
- Program.cs / startup code
- DTO property getters/setters

### Viewing Coverage Reports

```bash
# Generate coverage
dotnet test --collect:"XPlat Code Coverage"

# Install ReportGenerator (one-time)
dotnet tool install -g dotnet-reportgenerator-globaltool

# Generate HTML report
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html

# Open in browser
open coveragereport/index.html  # macOS
start coveragereport/index.html # Windows
xdg-open coveragereport/index.html # Linux
```

---

## Test Checklist

Before considering tests complete:

- [ ] All 7 endpoints have at least 3 tests each (21+ total)
- [ ] All tests follow AAA pattern with clear sections
- [ ] All test names follow `Method_Scenario_Behavior` convention
- [ ] All tests use FluentAssertions for assertions
- [ ] All async methods are awaited (no `.Result` or `.Wait()`)
- [ ] All mocks are isolated per test (no shared state)
- [ ] Test execution completes in under 2 seconds
- [ ] Code coverage is 80%+ for TasksController
- [ ] All tests pass consistently (no flaky tests)

---

## Resources

- **xUnit Documentation**: https://xunit.net/docs/getting-started/netcore/cmdline
- **Moq Documentation**: https://github.com/moq/moq4
- **FluentAssertions Documentation**: https://fluentassertions.com/introduction
- **Coverlet Documentation**: https://github.com/coverlet-coverage/coverlet

---

## Next Steps

1. Create `TaskApi.Tests` project
2. Add NuGet packages (xUnit, Moq, FluentAssertions)
3. Create `TestDataFixtures.cs` helper class
4. Implement tests for each endpoint following patterns above
5. Run tests with coverage to verify 80%+ target
6. Integrate into CI/CD pipeline

**Ready to start testing!** 🚀
