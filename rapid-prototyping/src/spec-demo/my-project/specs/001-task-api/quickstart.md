# Quickstart Guide: Demo Task API

**Feature**: 001-task-api  
**Estimated Time**: 30-45 minutes  
**Last Updated**: 2025-11-15

---

## Prerequisites

- **.NET 8 SDK** installed ([Download here](https://dotnet.microsoft.com/download/dotnet/8.0))
- **Code Editor**: Visual Studio Code, Visual Studio 2022, or Rider
- **Terminal/Command Prompt** access
- **Web Browser** for testing Swagger UI

### Verify .NET Installation

```bash
dotnet --version
# Should output: 8.0.x or higher
```

---

## Project Setup (5 minutes)

### Step 1: Create New Web API Project

```bash
# Create project directory
mkdir TaskApi
cd TaskApi

# Create Web API project
dotnet new webapi -n TaskApi --no-https false

# Navigate into project
cd TaskApi
```

### Step 2: Add Required NuGet Packages

```bash
# Entity Framework Core In-Memory provider
dotnet add package Microsoft.EntityFrameworkCore.InMemory

# Swagger/OpenAPI (should already be included, but verify)
dotnet add package Swashbuckle.AspNetCore
```

### Step 3: Verify Project Structure

Your project should have:
```
TaskApi/
├── TaskApi.csproj
├── Program.cs
├── appsettings.json
├── appsettings.Development.json
└── WeatherForecast.cs (you can delete this example file)
```

---

## Implementation (30 minutes)

### Phase 1: Data Layer (10 minutes)

#### Create Folder Structure

```bash
# In TaskApi/ directory
mkdir -p Models/Entities Models/DTOs Data/Repositories Controllers
```

#### 1.1 Create Task Entity

Create `Models/Entities/TaskEntity.cs`:
```csharp
namespace TaskApi.Models.Entities
{
    public class TaskEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsComplete { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
```

#### 1.2 Create DbContext

Create `Data/TaskDbContext.cs`:
```csharp
using Microsoft.EntityFrameworkCore;
using TaskApi.Models.Entities;

namespace TaskApi.Data
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) { }
        
        public DbSet<TaskEntity> Tasks { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.IsComplete).HasDefaultValue(false);
            });
        }
    }
}
```

#### 1.3 Create Repository Interface

Create `Data/Repositories/ITaskRepository.cs`:
```csharp
using TaskApi.Models.Entities;

namespace TaskApi.Data.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskEntity>> GetAllAsync(string? status = null, string? search = null);
        Task<TaskEntity?> GetByIdAsync(Guid id);
        Task<TaskEntity> CreateAsync(TaskEntity task);
        Task<TaskEntity> UpdateAsync(TaskEntity task);
        Task DeleteAsync(Guid id);
    }
}
```

#### 1.4 Create Repository Implementation

Create `Data/Repositories/TaskRepository.cs`:
```csharp
using Microsoft.EntityFrameworkCore;
using TaskApi.Models.Entities;

namespace TaskApi.Data.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskDbContext _context;

        public TaskRepository(TaskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskEntity>> GetAllAsync(string? status = null, string? search = null)
        {
            IQueryable<TaskEntity> query = _context.Tasks;

            if (status == "complete") query = query.Where(t => t.IsComplete);
            if (status == "incomplete") query = query.Where(t => !t.IsComplete);
            if (!string.IsNullOrEmpty(search))
                query = query.Where(t => t.Title.Contains(search) || 
                                       (t.Description != null && t.Description.Contains(search)));

            return await query.OrderByDescending(t => t.CreatedAt).ToListAsync();
        }

        public async Task<TaskEntity?> GetByIdAsync(Guid id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task<TaskEntity> CreateAsync(TaskEntity task)
        {
            task.Id = Guid.NewGuid();
            task.CreatedAt = DateTime.UtcNow;
            task.UpdatedAt = DateTime.UtcNow;
            task.IsComplete = false;

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<TaskEntity> UpdateAsync(TaskEntity task)
        {
            task.UpdatedAt = DateTime.UtcNow;
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task DeleteAsync(Guid id)
        {
            var task = await GetByIdAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }
    }
}
```

### Phase 2: DTOs (5 minutes)

#### 2.1 Create TaskDto

Create `Models/DTOs/TaskDto.cs`:
```csharp
namespace TaskApi.Models.DTOs
{
    /// <summary>
    /// Represents a task in API responses
    /// </summary>
    public class TaskDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsComplete { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
```

#### 2.2 Create CreateTaskDto

Create `Models/DTOs/CreateTaskDto.cs`:
```csharp
using System.ComponentModel.DataAnnotations;

namespace TaskApi.Models.DTOs
{
    /// <summary>
    /// Input model for creating a new task
    /// </summary>
    public class CreateTaskDto
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
    }
}
```

#### 2.3 Create UpdateTaskDto

Create `Models/DTOs/UpdateTaskDto.cs`:
```csharp
using System.ComponentModel.DataAnnotations;

namespace TaskApi.Models.DTOs
{
    /// <summary>
    /// Input model for updating an existing task
    /// </summary>
    public class UpdateTaskDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 100 characters")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }
        public bool IsComplete { get; set; }
    }
}
```

### Phase 3: Controller (15 minutes)

Create `Controllers/TasksController.cs`:
```csharp
using Microsoft.AspNetCore.Mvc;
using TaskApi.Data.Repositories;
using TaskApi.Models.DTOs;
using TaskApi.Models.Entities;

namespace TaskApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _repository;

        public TasksController(ITaskRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Get all tasks with optional filtering
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TaskDto>), 200)]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetAll(
            [FromQuery] string? status = null, 
            [FromQuery] string? search = null)
        {
            var tasks = await _repository.GetAllAsync(status, search);
            return Ok(tasks.Select(MapToDto));
        }

        /// <summary>
        /// Get a task by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TaskDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TaskDto>> GetById(Guid id)
        {
            var task = await _repository.GetByIdAsync(id);
            if (task == null)
                return NotFound(new { detail = $"Task with id '{id}' was not found" });

            return Ok(MapToDto(task));
        }

        /// <summary>
        /// Create a new task
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(TaskDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<TaskDto>> Create([FromBody] CreateTaskDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                ModelState.AddModelError(nameof(dto.Title), "Title cannot be only whitespace");
                return BadRequest(ModelState);
            }

            var entity = new TaskEntity
            {
                Title = dto.Title.Trim(),
                Description = dto.Description?.Trim(),
                DueDate = dto.DueDate
            };

            var created = await _repository.CreateAsync(entity);
            var result = MapToDto(created);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update an existing task
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TaskDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TaskDto>> Update(Guid id, [FromBody] UpdateTaskDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                ModelState.AddModelError(nameof(dto.Title), "Title cannot be only whitespace");
                return BadRequest(ModelState);
            }

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { detail = $"Task with id '{id}' was not found" });

            existing.Title = dto.Title.Trim();
            existing.Description = dto.Description?.Trim();
            existing.DueDate = dto.DueDate;
            existing.IsComplete = dto.IsComplete;

            var updated = await _repository.UpdateAsync(existing);
            return Ok(MapToDto(updated));
        }

        /// <summary>
        /// Delete a task
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { detail = $"Task with id '{id}' was not found" });

            await _repository.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Mark a task as complete
        /// </summary>
        [HttpPatch("{id}/complete")]
        [ProducesResponseType(typeof(TaskDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TaskDto>> MarkComplete(Guid id)
        {
            var task = await _repository.GetByIdAsync(id);
            if (task == null)
                return NotFound(new { detail = $"Task with id '{id}' was not found" });

            task.IsComplete = true;
            var updated = await _repository.UpdateAsync(task);
            return Ok(MapToDto(updated));
        }

        /// <summary>
        /// Mark a task as incomplete
        /// </summary>
        [HttpPatch("{id}/incomplete")]
        [ProducesResponseType(typeof(TaskDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TaskDto>> MarkIncomplete(Guid id)
        {
            var task = await _repository.GetByIdAsync(id);
            if (task == null)
                return NotFound(new { detail = $"Task with id '{id}' was not found" });

            task.IsComplete = false;
            var updated = await _repository.UpdateAsync(task);
            return Ok(MapToDto(updated));
        }

        private static TaskDto MapToDto(TaskEntity entity) => new TaskDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            DueDate = entity.DueDate,
            IsComplete = entity.IsComplete,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}
```

### Phase 4: Configure Dependency Injection

Edit `Program.cs`:
```csharp
using Microsoft.EntityFrameworkCore;
using TaskApi.Data;
using TaskApi.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Demo Task API", Version = "v1" });
    
    // Enable XML comments for better Swagger documentation
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Register DbContext with In-Memory database
builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseInMemoryDatabase("TaskDb"));

// Register repository
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

### Phase 5: Enable XML Documentation

Edit `TaskApi.csproj` to add:
```xml
<PropertyGroup>
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
  <NoWarn>$(NoWarn);1591</NoWarn>
</PropertyGroup>
```

---

## Running the API (2 minutes)

### Start the Application

```bash
dotnet run
```

You should see output like:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
      Now listening on: https://localhost:5001
```

### Access Swagger UI

Open your browser to:
```
http://localhost:5000/swagger
```
or
```
https://localhost:5001/swagger
```

---

## Testing the API (5-10 minutes)

### Test 1: Create a Task

1. In Swagger UI, find `POST /api/tasks`
2. Click "Try it out"
3. Enter request body:
```json
{
  "title": "Complete workshop exercise",
  "description": "Build a task API with AI assistance",
  "dueDate": "2024-12-01T10:00:00Z"
}
```
4. Click "Execute"
5. Verify: 201 Created response with task including ID and timestamps

### Test 2: Get All Tasks

1. Find `GET /api/tasks`
2. Click "Try it out" → "Execute"
3. Verify: Array containing your created task

### Test 3: Get Task by ID

1. Copy the ID from the previous response
2. Find `GET /api/tasks/{id}`
3. Paste the ID and execute
4. Verify: 200 OK with task details

### Test 4: Update Task

1. Find `PUT /api/tasks/{id}`
2. Enter the task ID and update body:
```json
{
  "title": "Updated task title",
  "description": "Updated description",
  "dueDate": "2024-12-02T10:00:00Z",
  "isComplete": true
}
```
3. Verify: 200 OK with updated values and new UpdatedAt timestamp

### Test 5: Mark Complete/Incomplete

1. Create a new task (POST)
2. Use `PATCH /api/tasks/{id}/complete` → Verify isComplete = true
3. Use `PATCH /api/tasks/{id}/incomplete` → Verify isComplete = false

### Test 6: Filter and Search

1. Create multiple tasks (some complete, some not)
2. Test `GET /api/tasks?status=complete`
3. Test `GET /api/tasks?status=incomplete`
4. Test `GET /api/tasks?search=workshop`

### Test 7: Delete Task

1. Find `DELETE /api/tasks/{id}`
2. Enter a task ID and execute
3. Verify: 204 No Content
4. Try to GET the same task → Verify: 404 Not Found

### Test 8: Validation

1. Try to create a task with empty title → Verify: 400 Bad Request
2. Try to create a task with title >100 characters → Verify: 400 Bad Request
3. Try to create a task with description >500 characters → Verify: 400 Bad Request

---

## Troubleshooting

### Issue: "No route matches the supplied values"
- Ensure controller route is `[Route("api/[controller]")]`
- Check that action names match the `CreatedAtAction` call

### Issue: XML comments not showing in Swagger
- Verify `<GenerateDocumentationFile>true</GenerateDocumentationFile>` in .csproj
- Rebuild project: `dotnet build`

### Issue: Validation not working
- Ensure `[ApiController]` attribute is on the controller
- Check that Data Annotations are on DTO properties

### Issue: 500 Internal Server Error
- Check console output for exception details
- Verify DbContext is registered in DI container

---

## Success Criteria Checklist

- ✅ All 7 endpoints functional (GET all, GET by id, POST, PUT, DELETE, PATCH complete, PATCH incomplete)
- ✅ Swagger UI accessible at /swagger
- ✅ Create task returns 201 with Location header
- ✅ Validation errors return 400 with details
- ✅ Not found returns 404
- ✅ Timestamps automatically managed
- ✅ Filtering by status works
- ✅ Search by text works
- ✅ Repository pattern with DI implemented

---

## Next Steps (Post-Workshop)

1. **Add Unit Tests**: Test repository with In-Memory DbContext
2. **Add Integration Tests**: Test controllers with WebApplicationFactory
3. **Switch to SQL Database**: Replace In-Memory with SQL Server/PostgreSQL
4. **Add Authentication**: Implement JWT authentication
5. **Add Pagination**: Implement skip/take for large result sets
6. **Add CORS**: Enable cross-origin requests for frontend
7. **Add Logging**: Use ILogger for diagnostics
8. **Add Health Checks**: Implement /health endpoint

---

## Resources

- [ASP.NET Core Documentation](https://learn.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core Documentation](https://learn.microsoft.com/en-us/ef/core/)
- [OpenAPI/Swagger Documentation](https://swagger.io/docs/)
- [REST API Best Practices](https://github.com/microsoft/api-guidelines)
