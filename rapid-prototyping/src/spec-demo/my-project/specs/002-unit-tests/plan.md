# Implementation Plan: API Unit Testing Coverage

**Branch**: `002-unit-tests` | **Date**: 2025-11-15 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/002-unit-tests/spec.md`

## Summary

Create comprehensive unit tests for all seven TasksController API endpoints (GetAll, Create, GetById, Update, Delete, MarkComplete, MarkIncomplete) to verify correct behavior with mocked dependencies. Tests must validate success paths, error handling, input validation, and DTO mapping without requiring database or external dependencies. Target 80%+ code coverage with sub-2-second execution time.

## Technical Context

**Language/Version**: C# / .NET 9.0  
**Primary Dependencies**: ASP.NET Core 9.0, Entity Framework Core 9.0 (In-Memory), Swashbuckle  
**Storage**: In-Memory database (EF Core InMemory provider)  
**Testing**: xUnit (to be added), Moq (to be added), FluentAssertions (to be added)  
**Target Platform**: Cross-platform (.NET 9 runtime)  
**Project Type**: Web API (backend only)  
**Performance Goals**: <2 seconds total test execution time, <100ms per individual test  
**Constraints**: Tests must run in isolation without external dependencies (database, network, file system)  
**Scale/Scope**: 21+ unit tests (3+ per endpoint × 7 endpoints), 80%+ code coverage of TasksController

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

### ✅ Principle I: Repository Pattern & Dependency Injection
**Status**: COMPLIANT  
**Evidence**: TasksController already uses ITaskRepository injected via DI. Unit tests will mock this interface, perfectly aligning with the repository pattern for testability.

### ✅ Principle II: Input Validation (NON-NEGOTIABLE)
**Status**: COMPLIANT  
**Evidence**: Unit tests will verify all existing validation rules (FR-005, FR-007) including title whitespace validation and ModelState error handling. Tests ensure validation is enforced.

### ✅ Principle III: Entity Framework Core Data Access
**Status**: COMPLIANT  
**Evidence**: Tests operate at the controller layer with mocked repositories. EF Core usage remains in repository implementations (not modified by this feature).

### ✅ Principle IV: OpenAPI Documentation (NON-NEGOTIABLE)
**Status**: COMPLIANT  
**Evidence**: Existing endpoints already have ProducesResponseType attributes and XML documentation. Tests will verify response types match documentation (FR-002, FR-003).

### ✅ Principle V: Testing & Quality Assurance
**Status**: **PRIMARY FOCUS OF THIS FEATURE**  
**Evidence**: This feature directly implements constitutional requirements:
- "All repository implementations MUST have unit tests with mocked DbContext" → Tests will mock ITaskRepository
- "All services with business logic MUST have unit tests" → Controller logic will be unit tested
- "Test projects MUST use xUnit, NUnit, or MSTest" → Will use xUnit
- "Code coverage metrics SHOULD be tracked (target: >80% for critical paths)" → Target 80%+ for TasksController

**Gate Result**: ✅ **PASSED** - All constitutional principles aligned. This feature enhances compliance with Principle V.

---

**POST-DESIGN RE-EVALUATION** (After Phase 1):

All constitution checks remain **COMPLIANT** after completing research and design phases:

### ✅ Principle I - Repository Pattern: REAFFIRMED
**Design Impact**: Test project structure (`TaskApi.Tests`) creates mocks of `ITaskRepository`, demonstrating the value of the repository abstraction. No changes to production repository pattern.

### ✅ Principle II - Input Validation: REAFFIRMED  
**Design Impact**: Test cases explicitly verify validation behavior (FR-005, FR-007). Research identified FluentAssertions patterns for asserting validation errors. Tests will document validation requirements.

### ✅ Principle III - EF Core: REAFFIRMED
**Design Impact**: Tests don't touch EF Core layer. Repository mocks isolate controller logic from data access, preserving EF Core usage in repository implementations.

### ✅ Principle IV - OpenAPI Documentation: REAFFIRMED
**Design Impact**: Tests verify `ProducesResponseType` attributes match actual responses (FR-002, FR-003), ensuring OpenAPI documentation accuracy.

### ✅ Principle V - Testing & Quality Assurance: **FULLY IMPLEMENTED**
**Design Impact**: 
- ✅ xUnit 2.6.2 selected (constitutional requirement)
- ✅ Moq 4.20.70 for mocking (aligns with "mocked DbContext" principle)
- ✅ FluentAssertions 6.12.0 for assertions (improves test readability)
- ✅ Coverlet 6.0.0 for coverage (meets ">80% for critical paths" requirement)
- ✅ AAA pattern enforced (constitutional best practice)
- ✅ Test organization follows .NET conventions (separate test project)

**Final Gate Result**: ✅ **PASSED** - No constitutional violations. Feature implementation strengthens constitutional compliance for Principle V.

## Project Structure

### Documentation (this feature)

```text
specs/002-unit-tests/
├── plan.md              # This file (/speckit.plan command output)
├── research.md          # Phase 0 output - testing framework selection & best practices
├── data-model.md        # N/A - no new data entities
├── quickstart.md        # Phase 1 output - how to run tests, write new tests
├── contracts/           # N/A - no API contracts (testing existing endpoints)
├── checklists/
│   └── requirements.md  # Specification validation checklist
└── spec.md              # Feature specification (already created)
```

### Source Code (repository root)

```text
TaskApi/                          # Existing API project
├── Controllers/
│   └── TasksController.cs       # Target for unit testing (no changes)
├── Data/
│   └── Repositories/
│       ├── ITaskRepository.cs   # Will be mocked in tests
│       └── TaskRepository.cs    # Not directly tested (unit tests focus on controller)
├── Models/
│   ├── DTOs/                    # Used in test assertions
│   │   ├── CreateTaskDto.cs
│   │   ├── TaskDto.cs
│   │   └── UpdateTaskDto.cs
│   └── Entities/
│       └── TaskEntity.cs        # Used in mock data
├── Program.cs
└── TaskApi.csproj               # Update: add test framework package references

TaskApi.Tests/                   # NEW: Test project
├── TaskApi.Tests.csproj         # NEW: Test project file with xUnit, Moq, FluentAssertions
├── Controllers/
│   └── TasksControllerTests.cs  # NEW: 21+ unit tests for all endpoints
├── Fixtures/
│   └── TestDataFixtures.cs      # NEW: Reusable test data (sample tasks, DTOs)
└── Mocks/
    └── MockTaskRepository.cs    # NEW: Helper for creating mock repository instances
```

**Structure Decision**: Following .NET convention of separate test project (`TaskApi.Tests`) alongside production project (`TaskApi`). Test project references production project and includes testing frameworks. Tests organized by controller namespace mirroring production code structure.

## Complexity Tracking

> **Fill ONLY if Constitution Check has violations that must be justified**

**N/A** - No constitutional violations. All gates passed.
````
