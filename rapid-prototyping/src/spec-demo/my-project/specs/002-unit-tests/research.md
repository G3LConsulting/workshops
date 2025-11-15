# Research: API Unit Testing Coverage

**Feature**: 002-unit-tests  
**Date**: 2025-11-15  
**Phase**: 0 - Research & Discovery

---

## Research Questions

From Technical Context in `plan.md`, the following needed clarification:

1. **Testing Framework Selection**: Which xUnit features and patterns are best for ASP.NET Core controller testing?
2. **Mocking Strategy**: Best practices for mocking ITaskRepository with Moq
3. **Assertion Library**: FluentAssertions patterns for API response validation
4. **Test Organization**: How to structure test classes and test methods for maintainability
5. **Code Coverage**: Tools and techniques for measuring and reporting coverage in .NET 9

---

## Decision 1: Testing Framework - xUnit

**Decision**: Use xUnit 2.6+ as the primary testing framework

**Rationale**:
- **Constitutional alignment**: Constitution V specifies "xUnit, NUnit, or MSTest" - xUnit chosen as it's most popular in .NET community
- **Parallelization**: xUnit runs tests in parallel by default, meeting our <2 second execution goal
- **Isolation**: Each test class gets its own instance, preventing test pollution
- **Theory support**: xUnit's `[Theory]` and `[InlineData]` ideal for testing multiple input scenarios efficiently
- **Ecosystem**: Excellent integration with Moq and FluentAssertions

**Alternatives Considered**:
- **NUnit**: More verbose setup (`[SetUp]`, `[TearDown]`), shared test fixtures increase risk of state pollution
- **MSTest**: Less popular in modern .NET, fewer community resources
- **Why xUnit wins**: Built-in parallelization, cleaner syntax, better suited for isolated unit tests

**Implementation Details**:
```xml
<PackageReference Include="xunit" Version="2.6.2" />
<PackageReference Include="xunit.runner.visualstudio" Version="2.5.4" />
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.8.0" />
```

---

## Decision 2: Mocking Framework - Moq

**Decision**: Use Moq 4.20+ for mocking ITaskRepository

**Rationale**:
- **Simple syntax**: `Mock<ITaskRepository>` with fluent setup API is readable and maintainable
- **Verification**: Can verify mock methods were called with expected parameters (critical for FR-008 DTO mapping tests)
- **Returns setup**: Easily configure mock to return specific test data or throw exceptions
- **Async support**: First-class support for `async Task<T>` returns needed for repository methods
- **Popular**: Most widely used .NET mocking library, extensive documentation and examples

**Alternatives Considered**:
- **NSubstitute**: Slightly cleaner syntax but less flexible verification capabilities
- **FakeItEasy**: Similar to NSubstitute, less common in enterprise .NET
- **Manual fakes**: Would require implementing full ITaskRepository interface for each test scenario (time-consuming, error-prone)
- **Why Moq wins**: Industry standard, powerful verification, excellent async support

**Key Patterns**:
```csharp
// Setup: Return specific data
mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
    .ReturnsAsync(testTask);

// Setup: Return null (404 scenarios)
mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
    .ReturnsAsync((TaskEntity?)null);

// Verification: Ensure method called with specific value
mockRepo.Verify(r => r.CreateAsync(It.Is<TaskEntity>(t => t.Title == "Test")), Times.Once);
```

**Implementation Details**:
```xml
<PackageReference Include="Moq" Version="4.20.70" />
```

---

## Decision 3: Assertion Library - FluentAssertions

**Decision**: Use FluentAssertions 6.12+ for all test assertions

**Rationale**:
- **Readability**: Natural language assertions like `result.Should().BeOfType<OkObjectResult>()` are self-documenting
- **Rich API responses**: Excellent support for asserting on complex objects (DTOs, ActionResult types)
- **Error messages**: Detailed failure messages show exactly what differed, speeding up debugging
- **Collections**: Strong support for asserting on IEnumerable (needed for GetAll tests with filtering)
- **Status codes**: Can assert on HTTP status codes from ActionResult types directly

**Alternatives Considered**:
- **xUnit Assert**: Functional but verbose (`Assert.Equal(expected, actual)` vs `actual.Should().Be(expected)`)
- **Shouldly**: Similar to FluentAssertions but less mature .NET Core support
- **Why FluentAssertions wins**: Best-in-class for complex object assertions, superior error messages, excellent for API testing

**Key Patterns**:
```csharp
// ActionResult type assertions
var result = await controller.Create(dto);
result.Should().BeOfType<CreatedAtActionResult>();

// Status code assertions
var okResult = result as OkObjectResult;
okResult.StatusCode.Should().Be(200);

// DTO property assertions
var taskDto = okResult.Value as TaskDto;
taskDto.Should().NotBeNull();
taskDto.Title.Should().Be("Expected Title");
taskDto.IsComplete.Should().BeFalse();

// Collection assertions
var tasks = result.Value as IEnumerable<TaskDto>;
tasks.Should().HaveCount(3);
tasks.Should().OnlyContain(t => t.IsComplete);
```

**Implementation Details**:
```xml
<PackageReference Include="FluentAssertions" Version="6.12.0" />
```

---

## Decision 4: Test Organization - AAA Pattern

**Decision**: Organize all tests using Arrange-Act-Assert (AAA) pattern with clear test naming

**Rationale**:
- **Constitutional requirement**: FR-013 mandates AAA pattern
- **Readability**: Three-phase structure makes test intent obvious at a glance
- **Maintainability**: Clear separation between setup, execution, and verification
- **Debugging**: Easy to identify which phase failed when test breaks

**Test Naming Convention**:
```
MethodName_Scenario_ExpectedBehavior
```

Examples:
- `Create_WithValidDto_Returns201Created`
- `Create_WithWhitespaceTitle_Returns400BadRequest`
- `GetById_WithNonExistentId_Returns404NotFound`
- `GetAll_WithStatusFilter_ReturnsOnlyMatchingTasks`

**Test Class Organization**:
```csharp
public class TasksControllerTests
{
    // Nested classes group related tests
    public class CreateTests
    {
        [Fact]
        public async Task Create_WithValidDto_Returns201Created()
        {
            // Arrange: Setup mock, create controller, prepare test data
            var mockRepo = new Mock<ITaskRepository>();
            var controller = new TasksController(mockRepo.Object);
            var dto = new CreateTaskDto { Title = "Test Task" };
            
            // Act: Call the method under test
            var result = await controller.Create(dto);
            
            // Assert: Verify the outcome
            result.Should().BeOfType<CreatedAtActionResult>();
        }
    }
    
    public class GetByIdTests { /* ... */ }
    public class UpdateTests { /* ... */ }
    // etc.
}
```

**Alternatives Considered**:
- **One test per file**: Too granular, 21+ files hard to navigate
- **Flat structure**: All 21+ tests in one class would be overwhelming
- **Why nested classes win**: Balances organization with discoverability, aligns with xUnit's parallel execution

---

## Decision 5: Code Coverage - Built-in .NET Coverage Tools

**Decision**: Use `dotnet test --collect:"XPlat Code Coverage"` with Coverlet

**Rationale**:
- **No additional tools**: Coverlet is standard in .NET SDK, no external dependencies
- **CI/CD friendly**: CLI-based, easy to integrate into build pipelines
- **Coverage formats**: Generates Cobertura XML and JSON reports for analysis
- **Line + branch coverage**: Tracks both line execution and conditional branch coverage
- **Target verification**: Can enforce 80% minimum threshold in build (SC-002)

**Alternatives Considered**:
- **Visual Studio Code Coverage**: IDE-only, not cross-platform
- **OpenCover**: Older tool, less maintained
- **Why Coverlet wins**: Modern, built-in, cross-platform, standard in .NET ecosystem

**Implementation**:
```bash
# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Generate HTML report (requires ReportGenerator)
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html

# Verify 80% threshold in CI
dotnet test /p:CollectCoverage=true /p:Threshold=80 /p:ThresholdType=line
```

**Coverage Configuration** (.runsettings):
```xml
<RunSettings>
  <DataCollectionRunSettings>
    <DataCollectors>
      <DataCollector friendlyName="XPlat Code Coverage">
        <Configuration>
          <Include>[TaskApi]*</Include>
          <Exclude>[TaskApi]*.Program,[TaskApi]*.Startup</Exclude>
        </Configuration>
      </DataCollector>
    </DataCollectors>
  </DataCollectionRunSettings>
</RunSettings>
```

**Package Requirements**:
```xml
<PackageReference Include="coverlet.collector" Version="6.0.0" />
```

---

## Best Practices Research

### Controller Testing Patterns

**Mock Repository Setup**:
- Create fresh `Mock<ITaskRepository>` for each test (avoid shared state)
- Use `It.IsAny<>` for parameters you don't care about
- Use `It.Is<>` with lambda for precise parameter validation
- Always setup async methods with `.ReturnsAsync()` not `.Returns()`

**ActionResult Unwrapping**:
```csharp
// Pattern for extracting value from ActionResult<T>
var actionResult = await controller.GetById(id);
var okResult = actionResult.Result.Should().BeOfType<OkObjectResult>().Subject;
var dto = okResult.Value.Should().BeOfType<TaskDto>().Subject;
dto.Id.Should().Be(id);
```

**ModelState Testing**:
```csharp
// Simulate validation failure
controller.ModelState.AddModelError("Title", "Title is required");

// Or test with invalid ModelState
var dto = new CreateTaskDto(); // Missing required Title
controller.ValidateModel(dto); // Extension method to trigger validation
```

### Test Data Management

**Fixture Pattern**:
```csharp
public static class TestDataFixtures
{
    public static TaskEntity SampleTask() => new()
    {
        Id = Guid.NewGuid(),
        Title = "Sample Task",
        Description = "Sample description",
        IsComplete = false,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    };
    
    public static TaskEntity SampleTask(Action<TaskEntity> customize)
    {
        var task = SampleTask();
        customize(task);
        return task;
    }
}

// Usage: var task = TestDataFixtures.SampleTask(t => t.IsComplete = true);
```

### Performance Optimization

**Parallel Execution**:
- xUnit runs test classes in parallel by default
- Each test class gets isolated fixture
- Use `[Collection]` attribute only if tests truly share state (not needed for our unit tests)

**Async Best Practices**:
- Always `await` async controller methods (don't use `.Result` or `.Wait()`)
- Use `Task.FromResult()` for synchronous mock returns that need to be async
- Mark test methods as `async Task`, never `async void`

---

## Technology Stack Summary

| Component | Technology | Version | Purpose |
|-----------|-----------|---------|---------|
| Test Framework | xUnit | 2.6.2 | Test execution, assertions, parallel running |
| Mocking | Moq | 4.20.70 | Mock ITaskRepository interface |
| Assertions | FluentAssertions | 6.12.0 | Readable, detailed assertions |
| Coverage | Coverlet | 6.0.0 | Code coverage collection |
| Test Runner | Microsoft.NET.Test.Sdk | 17.8.0 | .NET CLI test execution |
| IDE Runner | xunit.runner.visualstudio | 2.5.4 | Visual Studio Test Explorer integration |

---

## Risks & Mitigations

### Risk 1: Test Execution Time Exceeds 2 Seconds
**Mitigation**: 
- Leverage xUnit parallel execution (enabled by default)
- Keep tests isolated (no shared fixtures that serialize execution)
- Avoid Thread.Sleep or Task.Delay in tests

### Risk 2: Mocks Don't Match Real Repository Behavior
**Mitigation**:
- Document repository contract expectations in tests
- Consider integration tests in future feature to validate repository implementations
- Keep mock setups simple and focused on single behavior per test

### Risk 3: Coverage False Positives
**Mitigation**:
- Focus on branch coverage, not just line coverage
- Test both success and error paths for every endpoint
- Verify coverage reports manually, don't just trust metrics

---

## Next Steps

Phase 0 research complete. All technical unknowns resolved:
- ✅ Testing framework selected (xUnit)
- ✅ Mocking strategy defined (Moq)
- ✅ Assertion library chosen (FluentAssertions)
- ✅ Test organization pattern decided (AAA with nested classes)
- ✅ Coverage tooling identified (Coverlet)

**Ready for Phase 1**: Design test structure, create test project, define test data fixtures.
