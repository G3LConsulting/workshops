# Implementation Plan: Demo Task API

**Branch**: `001-task-api` | **Date**: 2025-11-15 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/001-task-api/spec.md`

**Note**: This template is filled in by the `/speckit.plan` command. See `.specify/templates/commands/plan.md` for the execution workflow.

## Summary

Implement a REST API for task management demonstrating core CRUD operations, input validation, and OpenAPI documentation. The API will use .NET 8 LTS with Entity Framework Core In-Memory database, repository pattern with dependency injection, and comprehensive validation. This is a 30-45 minute workshop exercise focusing on spec-driven development with AI assistance, implementing 7 endpoints (GET all, GET by id, POST, PUT, DELETE, PATCH complete/incomplete) with automatic timestamp management and filtering capabilities.

## Technical Context

**Language/Version**: C# 12 / .NET 8 LTS  
**Primary Dependencies**: ASP.NET Core 8.0, Entity Framework Core 8.0 (In-Memory provider), Swashbuckle.AspNetCore 6.5+  
**Storage**: Entity Framework Core In-Memory Database (for workshop simplicity, production would use SQL Server/PostgreSQL)  
**Testing**: xUnit 2.6+ (unit tests optional for workshop, focus on manual Swagger testing)  
**Target Platform**: Cross-platform (Windows, macOS, Linux) - runs as web API service
**Project Type**: Single backend API project (no frontend)  
**Performance Goals**: Not applicable for workshop demo (handles typical workshop load <10 concurrent users)  
**Constraints**: Must complete implementation in <45 minutes, minimal setup required, no external database dependencies  
**Scale/Scope**: Workshop demonstration with ~100 LOC for models, ~200 LOC for repositories, ~300 LOC for controllers

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

### Principle I: Repository Pattern & Dependency Injection ✅
- **Requirement**: All data access through repository pattern with DI
- **Status**: PASS - Design includes ITaskRepository interface with TaskRepository implementation, registered in DI container
- **Implementation**: Repository handles all EF Core operations, controllers depend only on interface

### Principle II: Input Validation (NON-NEGOTIABLE) ✅
- **Requirement**: All REST requests must be validated
- **Status**: PASS - 9 validation requirements defined (VR-001 through VR-009)
- **Implementation**: Data Annotations on DTOs with [ApiController] attribute for automatic validation, custom validation for whitespace-only titles

### Principle III: Entity Framework Core Data Access ✅
- **Requirement**: EF Core exclusive ORM with migrations
- **Status**: PASS - Task entity with EF Core configuration, In-Memory provider for workshop
- **Note**: Migrations not applicable for In-Memory database, would be required for production SQL database

### Principle IV: OpenAPI Documentation (NON-NEGOTIABLE) ✅
- **Requirement**: Complete OpenAPI/Swagger documentation
- **Status**: PASS - FR-013 and SC-004 mandate Swagger with full endpoint documentation
- **Implementation**: Swashbuckle configured, XML comments enabled, ProducesResponseType attributes on all endpoints

### Principle V: Testing & Quality Assurance ⚠️
- **Requirement**: Unit and integration tests
- **Status**: PARTIAL - Unit tests optional for workshop time constraint, manual testing via Swagger mandatory
- **Justification**: 45-minute workshop prioritizes functional implementation and manual validation over automated tests
- **Production Note**: Full test suite would be required before production deployment

### Overall Assessment: ✅ PASS WITH EXCEPTION

**Decision**: Proceed with implementation. Testing principle deviation justified by workshop educational goals and time constraints. All other principles fully satisfied.

## Project Structure

### Documentation (this feature)

```text
specs/001-task-api/
├── plan.md              # This file (/speckit.plan command output)
├── research.md          # Phase 0 output (/speckit.plan command)
├── data-model.md        # Phase 1 output (/speckit.plan command)
├── quickstart.md        # Phase 1 output (/speckit.plan command)
├── contracts/           # Phase 1 output (/speckit.plan command)
│   └── openapi.yaml     # OpenAPI 3.0 specification
└── tasks.md             # Phase 2 output (/speckit.tasks command - NOT created by /speckit.plan)
```

### Source Code (repository root)

```text
TaskApi/                         # .NET 8 Web API project
├── TaskApi.csproj              # Project file with package references
├── Program.cs                  # Application entry point, DI configuration
├── appsettings.json            # Configuration
├── Models/
│   ├── Entities/
│   │   └── Task.cs            # EF Core entity
│   └── DTOs/
│       ├── TaskDto.cs         # Response DTO
│       ├── CreateTaskDto.cs   # Create request DTO with validation
│       └── UpdateTaskDto.cs   # Update request DTO with validation
├── Data/
│   ├── TaskDbContext.cs       # EF Core DbContext
│   └── Repositories/
│       ├── ITaskRepository.cs      # Repository interface
│       └── TaskRepository.cs       # Repository implementation
└── Controllers/
    └── TasksController.cs     # REST API controller with 7 endpoints

TaskApi.Tests/                  # Test project (optional for workshop)
├── TaskApi.Tests.csproj
├── Unit/
│   └── TaskRepositoryTests.cs
└── Integration/
    └── TasksControllerTests.cs
```

**Structure Decision**: Selected single project structure (Option 1) as this is a backend-only API with no frontend component. The structure follows standard ASP.NET Core conventions with separation of concerns: Models for data shapes, Data layer for persistence, and Controllers for API endpoints. Tests are in a separate project following .NET naming conventions (ProjectName.Tests).

## Complexity Tracking

> **Fill ONLY if Constitution Check has violations that must be justified**

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| Testing Principle (Partial) | 45-minute workshop time constraint requires focus on implementation over automated testing | Full TDD cycle would extend workshop to 2+ hours, defeating educational goal of rapid prototyping demonstration. Manual Swagger testing provides immediate feedback and validates functionality. |

**Note**: This is the only deviation from constitutional principles. All other principles (Repository Pattern, Input Validation, EF Core, OpenAPI Documentation) are fully implemented.
