# Data Model: Demo Task API

**Feature**: 001-task-api  
**Date**: 2025-11-15  
**Status**: Complete

---

## Overview

This document defines the data model for the Task Management API, including entity definitions, relationships, validation rules, and state transitions.

---

## Entities

### Task Entity

**Purpose**: Represents a single to-do item in the task management system

**Table Name**: `Tasks` (EF Core convention)

**Primary Key**: `Id` (Guid)

#### Fields

| Field Name | Type | Nullable | Default | Description |
|------------|------|----------|---------|-------------|
| `Id` | Guid | No | Auto-generated | Unique identifier for the task |
| `Title` | string | No | - | Task name/summary (1-100 characters) |
| `Description` | string | Yes | null | Detailed task description (max 500 characters) |
| `DueDate` | DateTime? | Yes | null | Optional deadline for task completion (UTC) |
| `IsComplete` | bool | No | false | Task completion status |
| `CreatedAt` | DateTime | No | Auto-generated | Timestamp when task was created (UTC) |
| `UpdatedAt` | DateTime | No | Auto-generated | Timestamp when task was last modified (UTC) |

#### Entity Code (EF Core)

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

#### EF Core Configuration

```csharp
public class TaskDbContext : DbContext
{
    public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) { }
    
    public DbSet<TaskEntity> Tasks { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(e => e.Description)
                .HasMaxLength(500);
            
            entity.Property(e => e.IsComplete)
                .IsRequired()
                .HasDefaultValue(false);
            
            entity.Property(e => e.CreatedAt)
                .IsRequired();
            
            entity.Property(e => e.UpdatedAt)
                .IsRequired();
        });
        
        base.OnModelCreating(modelBuilder);
    }
}
```

---

## Data Transfer Objects (DTOs)

### TaskDto (Response DTO)

**Purpose**: Complete task representation for API responses

**Used in**: GET responses, POST/PUT/PATCH responses

```csharp
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
```

### CreateTaskDto (Request DTO)

**Purpose**: Input for creating new tasks

**Used in**: POST /api/tasks

**Validation Rules**: VR-001, VR-002, VR-003, VR-005, VR-007

```csharp
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

### UpdateTaskDto (Request DTO)

**Purpose**: Input for updating existing tasks

**Used in**: PUT /api/tasks/{id}

**Validation Rules**: VR-001, VR-002, VR-003, VR-005, VR-007

```csharp
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
```

---

## Validation Rules

### Title Validation (VR-001, VR-002, VR-003)

**Rules**:
1. Title is required (`[Required]` attribute)
2. Title must be 1-100 characters (`[StringLength(100, MinimumLength = 1)]`)
3. Title cannot be only whitespace (custom validation needed)

**Implementation**:
```csharp
// In controller or custom validator
if (string.IsNullOrWhiteSpace(dto.Title))
{
    ModelState.AddModelError(nameof(dto.Title), "Title cannot be only whitespace");
    return BadRequest(ModelState);
}
```

**Error Response Example**:
```json
{
  "type": "ValidationError",
  "title": "One or more validation errors occurred",
  "status": 400,
  "errors": {
    "title": ["Title is required", "Title cannot be only whitespace"]
  }
}
```

### Description Validation (VR-004, VR-005)

**Rules**:
1. Description is optional (nullable string)
2. Description max 500 characters when provided (`[MaxLength(500)]`)

**Automatic**: Handled by Data Annotations

### DueDate Validation (VR-006, VR-007)

**Rules**:
1. DueDate is optional (nullable DateTime)
2. DueDate must be valid DateTime when provided

**Automatic**: ASP.NET Core model binding validates DateTime format

### IsComplete Validation (VR-008)

**Rule**: Defaults to false for new tasks

**Implementation**: Set in repository's CreateAsync method

---

## State Transitions

### Task Lifecycle

```
[New Task Created]
     ↓
[IsComplete = false]  ←→  [PATCH /api/tasks/{id}/complete]
     ↓                    [PATCH /api/tasks/{id}/incomplete]
[IsComplete = true]
     ↓
[Can be updated or deleted at any time]
```

### Timestamp Management

**CreatedAt**:
- Set automatically when task is created
- Never changes after creation
- Set to `DateTime.UtcNow` in repository

**UpdatedAt**:
- Set automatically when task is created (same as CreatedAt initially)
- Updated to `DateTime.UtcNow` on every update (PUT, PATCH)
- Managed by repository layer

**Implementation**:
```csharp
// In TaskRepository.CreateAsync
var task = new TaskEntity
{
    Id = Guid.NewGuid(),
    Title = dto.Title,
    Description = dto.Description,
    DueDate = dto.DueDate,
    IsComplete = false,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow
};

// In TaskRepository.UpdateAsync
existingTask.UpdatedAt = DateTime.UtcNow;
```

---

## Relationships

**None**: This is a single-entity system with no foreign keys or navigation properties.

**Future Considerations** (not in workshop scope):
- User entity (task ownership)
- Category/Tag entities (task classification)
- TaskHistory entity (audit trail)

---

## Indexes

**In-Memory Database Note**: Indexes not applicable for workshop (In-Memory provider doesn't use indexes)

**Production Recommendations**:
```csharp
entity.HasIndex(e => e.IsComplete); // For status filtering
entity.HasIndex(e => e.CreatedAt);  // For sorting
entity.HasIndex(e => e.Title);      // For search
```

---

## Data Integrity Constraints

### Database-Level Constraints (EF Core)

1. **Primary Key**: `Id` (Guid) ensures uniqueness
2. **NOT NULL**: Title, IsComplete, CreatedAt, UpdatedAt
3. **String Lengths**: Title (100), Description (500)
4. **Default Value**: IsComplete defaults to false

### Application-Level Constraints

1. **Title Whitespace**: Validated in controller/service layer
2. **UTC Timestamps**: Enforced by using `DateTime.UtcNow`
3. **Guid Generation**: Enforced by using `Guid.NewGuid()`

---

## Mapping Strategy

### Entity → DTO Mapping

```csharp
// Manual mapping (simple, workshop-appropriate)
public static TaskDto ToDto(TaskEntity entity)
{
    return new TaskDto
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
```

### DTO → Entity Mapping

```csharp
// For Create
public static TaskEntity ToEntity(CreateTaskDto dto)
{
    return new TaskEntity
    {
        Id = Guid.NewGuid(),
        Title = dto.Title.Trim(), // Defensive trim
        Description = dto.Description?.Trim(),
        DueDate = dto.DueDate,
        IsComplete = false,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    };
}

// For Update (applies changes to existing entity)
public static void UpdateEntity(TaskEntity entity, UpdateTaskDto dto)
{
    entity.Title = dto.Title.Trim();
    entity.Description = dto.Description?.Trim();
    entity.DueDate = dto.DueDate;
    entity.IsComplete = dto.IsComplete;
    entity.UpdatedAt = DateTime.UtcNow;
}
```

---

## Summary

- **Single Entity**: Task (with 7 fields)
- **3 DTOs**: TaskDto, CreateTaskDto, UpdateTaskDto
- **9 Validation Rules**: All defined with Data Annotations and custom logic
- **Automatic Timestamps**: CreatedAt on create, UpdatedAt on create/update
- **No Relationships**: Simple, self-contained data model
- **UTC Enforcement**: All DateTime values stored in UTC

**Ready for implementation in Phase 1.**
