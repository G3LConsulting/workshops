# Research: Demo Task API

**Feature**: 001-task-api  
**Date**: 2025-11-15  
**Status**: Complete

---

## Overview

This document consolidates research findings and technical decisions for implementing the Demo Task API. All technical context items from the implementation plan have been resolved with concrete decisions.

---

## Technology Decisions

### Decision 1: .NET 8 LTS Framework

**What was chosen**: .NET 8 LTS with ASP.NET Core Minimal API or Controllers

**Rationale**:
- .NET 8 is the current LTS version (Long-Term Support until November 2026)
- Excellent performance and modern C# 12 features
- Built-in dependency injection container
- Native support for OpenAPI/Swagger via Swashbuckle
- Cross-platform support (Windows, macOS, Linux)
- Mature ecosystem with extensive documentation

**Alternatives considered**:
- .NET 7: Not LTS, support ends May 2024 (already past)
- .NET 6: Previous LTS, but .NET 8 offers better performance and features
- Node.js/Express: Would require different skillset, less type-safe
- Python/FastAPI: Excellent for APIs but not aligned with constitution's .NET focus

**Implementation notes**:
- Use `dotnet new webapi` template for project scaffolding
- Enable nullable reference types for better null safety
- Configure Kestrel web server with default settings

---

### Decision 2: Entity Framework Core In-Memory Database

**What was chosen**: EF Core 8.0 with In-Memory provider for workshop

**Rationale**:
- No external database setup required (critical for 45-minute workshop)
- Authentic EF Core experience (repository pattern, LINQ queries, change tracking)
- Zero configuration - works immediately after `dotnet run`
- Data persists for application lifetime (sufficient for workshop demos)
- Participants learn real EF Core patterns that transfer to SQL databases

**Alternatives considered**:
- SQL Server: Requires installation, connection strings, too complex for workshop
- SQLite: Requires file system, connection strings, migration overhead
- PostgreSQL: Same issues as SQL Server
- No database (in-memory collection): Loses educational value of EF Core patterns

**Implementation notes**:
- Use `Microsoft.EntityFrameworkCore.InMemory` NuGet package
- Register with `builder.Services.AddDbContext<TaskDbContext>(options => options.UseInMemoryDatabase("TaskDb"))`
- No migrations needed (In-Memory provider doesn't support migrations)
- For production, switch to SQL Server with migrations

---

### Decision 3: Data Annotations for Validation

**What was chosen**: ASP.NET Core Data Annotations with custom validation logic where needed

**Rationale**:
- Built into .NET, no additional packages required
- Works automatically with `[ApiController]` attribute
- Simple declarative syntax (`[Required]`, `[MaxLength(100)]`)
- Sufficient for workshop requirements (title length, required fields)
- Produces consistent 400 Bad Request responses with validation details

**Alternatives considered**:
- FluentValidation: More powerful but adds dependency and complexity for simple validation
- Manual validation in controllers: Violates DRY, error-prone, hard to maintain
- Custom validation attributes: Overkill for workshop scope

**Implementation notes**:
- Apply `[ApiController]` attribute to TasksController for automatic model validation
- Use `[Required]`, `[StringLength]`, `[MaxLength]` attributes on DTOs
- Custom validator for whitespace-only titles (cannot be done with attributes alone)
- Return 400 with ProblemDetails format for validation errors

---

### Decision 4: Swashbuckle for OpenAPI Documentation

**What was chosen**: Swashbuckle.AspNetCore 6.5+ for Swagger/OpenAPI generation

**Rationale**:
- De facto standard for .NET OpenAPI documentation
- Automatic UI at `/swagger` endpoint for interactive API testing
- Generates OpenAPI 3.0 specification from code annotations
- Supports XML comments for rich documentation
- Zero configuration needed beyond `builder.Services.AddSwaggerGen()`

**Alternatives considered**:
- NSwag: More features but more complex setup, overkill for workshop
- Manual OpenAPI YAML: Time-consuming, error-prone, defeats workshop purpose
- No documentation: Violates constitution principle IV (non-negotiable)

**Implementation notes**:
- Enable XML documentation generation in .csproj: `<GenerateDocumentationFile>true</GenerateDocumentationFile>`
- Configure Swagger in Program.cs with XML comments support
- Add `[ProducesResponseType]` attributes for each status code on controller actions
- Use /// XML comments on DTOs and controller methods

---

## Best Practices Research

### Best Practice 1: Repository Pattern with Async/Await

**Finding**: Repository pattern with async methods is essential for scalable web APIs

**Key patterns**:
```csharp
public interface ITaskRepository
{
    Task<IEnumerable<TaskEntity>> GetAllAsync(string? status = null, string? search = null);
    Task<TaskEntity?> GetByIdAsync(Guid id);
    Task<TaskEntity> CreateAsync(TaskEntity task);
    Task<TaskEntity> UpdateAsync(TaskEntity task);
    Task DeleteAsync(Guid id);
}
```

**Rationale**:
- Async methods prevent thread blocking during I/O operations
- Even In-Memory database benefits from async patterns (prepares for production SQL)
- `Task<T>` return types enable `await` in controllers
- Nullable reference types (`TaskEntity?`) express intent clearly

**Source**: Microsoft Docs - Entity Framework Core Best Practices, Async programming patterns

---

### Best Practice 2: DTO (Data Transfer Object) Pattern

**Finding**: Never expose entities directly through API endpoints

**Key patterns**:
- **CreateTaskDto**: Only fields client can provide (Title, Description, DueDate)
- **UpdateTaskDto**: Fields client can modify (includes IsComplete)
- **TaskDto**: Complete representation for responses (includes Id, timestamps)

**Rationale**:
- Decouples API contract from database schema
- Prevents over-posting attacks (client can't set Id, timestamps)
- Enables different validation rules for create vs update
- Allows API evolution without database changes

**Mapping strategy**: Manual mapping in repository/controller or AutoMapper for complex scenarios

**Source**: Microsoft REST API Guidelines, ASP.NET Core documentation

---

### Best Practice 3: Global Exception Handling

**Finding**: Use middleware for consistent error responses

**Key implementation**:
```csharp
app.UseExceptionHandler("/error");
app.MapGet("/error", () => Results.Problem("An error occurred"));
```

**Rationale**:
- Prevents 500 errors from exposing stack traces in production
- Consistent error format across all endpoints
- Centralized logging of unhandled exceptions
- Separates error handling from business logic

**For workshop**: Minimal implementation, log to console for debugging

**Source**: ASP.NET Core Error Handling documentation

---

### Best Practice 4: Dependency Injection Lifetimes

**Finding**: DbContext must be Scoped, Repositories should be Scoped, Controllers are Transient

**Key configuration**:
```csharp
builder.Services.AddDbContext<TaskDbContext>(options => 
    options.UseInMemoryDatabase("TaskDb")); // Scoped by default

builder.Services.AddScoped<ITaskRepository, TaskRepository>();
```

**Rationale**:
- Scoped: One instance per HTTP request (DbContext, Repositories)
- Transient: New instance each time (Controllers, lightweight services)
- Singleton: One instance for app lifetime (configuration, caches)

**DbContext rationale**: Must be scoped to avoid concurrency issues and ensure proper disposal

**Source**: Microsoft Dependency Injection guidelines

---

## Integration Patterns

### Pattern 1: Controller → Repository → DbContext Flow

**Flow diagram**:
```
HTTP Request → Controller (validates DTO) 
           → Repository (business logic, LINQ queries) 
           → DbContext (EF Core tracking, SaveChangesAsync)
           → Response DTO → HTTP Response
```

**Key points**:
- Controller handles HTTP concerns (status codes, content negotiation)
- Repository encapsulates data access logic
- DbContext manages entity tracking and database operations
- DTOs cross boundaries, entities stay in data layer

---

### Pattern 2: Filtering and Search Implementation

**Strategy**: Build dynamic LINQ queries based on optional parameters

**Example**:
```csharp
public async Task<IEnumerable<TaskEntity>> GetAllAsync(string? status, string? search)
{
    IQueryable<TaskEntity> query = _context.Tasks;
    
    if (status == "complete") query = query.Where(t => t.IsComplete);
    if (status == "incomplete") query = query.Where(t => !t.IsComplete);
    if (!string.IsNullOrEmpty(search))
        query = query.Where(t => t.Title.Contains(search) || t.Description.Contains(search));
    
    return await query.OrderByDescending(t => t.CreatedAt).ToListAsync();
}
```

**Rationale**: LINQ provides composable, type-safe queries that EF Core translates efficiently

---

## Workshop-Specific Considerations

### Consideration 1: Time-Boxed Implementation

**Finding**: Prioritize vertical slices over comprehensive coverage

**Strategy**:
- Phase 1 (15 min): Project setup, entity, DbContext, repository interface
- Phase 2 (15 min): Repository implementation, DI registration, first endpoint (POST)
- Phase 3 (10 min): Remaining CRUD endpoints (GET all, GET by id, PUT, DELETE)
- Phase 4 (5 min): PATCH endpoints, testing via Swagger

**Skip for workshop**: Unit tests, integration tests, advanced error handling, logging

---

### Consideration 2: Simplified Validation

**Finding**: Focus on demonstrating validation patterns, not edge cases

**Priority validations**:
1. Required title (most important)
2. Title length (1-100 characters)
3. Description length (max 500)
4. Automatic 400 Bad Request with clear messages

**Defer for workshop**: Whitespace-only title validation (nice-to-have), DueDate format validation (DateTime handles), concurrent update handling

---

## Summary of Resolved Items

| Technical Context Item | Resolution |
|------------------------|------------|
| Language/Version | C# 12 / .NET 8 LTS |
| Primary Dependencies | ASP.NET Core 8.0, EF Core 8.0 In-Memory, Swashbuckle |
| Storage | EF Core In-Memory Database |
| Testing | Manual via Swagger (automated tests optional) |
| Target Platform | Cross-platform web API |
| Project Type | Single backend API project |
| Performance Goals | Not applicable (workshop demo) |
| Constraints | 45-minute implementation, no external dependencies |
| Scale/Scope | ~600 LOC total, 7 endpoints |

**All NEEDS CLARIFICATION items resolved. Ready for Phase 1 design.**

---

## References

- [.NET 8 Documentation](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8)
- [Entity Framework Core In-Memory Database](https://learn.microsoft.com/en-us/ef/core/providers/in-memory/)
- [ASP.NET Core Web API Documentation](https://learn.microsoft.com/en-us/aspnet/core/web-api/)
- [Swashbuckle Documentation](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
- [Microsoft REST API Guidelines](https://github.com/microsoft/api-guidelines/blob/vNext/Guidelines.md)
