<!--
=============================================================================
SYNC IMPACT REPORT
=============================================================================
Version Change: [Initial] → 1.0.0
Change Type: MAJOR - Initial constitution ratification

Principles Defined:
  - I. Repository Pattern & Dependency Injection (NEW)
  - II. Input Validation (NON-NEGOTIABLE) (NEW)
  - III. Entity Framework Core Data Access (NEW)
  - IV. OpenAPI Documentation (NON-NEGOTIABLE) (NEW)
  - V. Testing & Quality Assurance (NEW)

Sections Added:
  - Technology Stack & Standards (NEW)
  - Development Workflow & Quality Gates (NEW)

Template Consistency Status:
  ✅ plan-template.md - Aligned with constitution principles (includes Constitution Check gate)
  ✅ spec-template.md - Requirements structure compatible with validation and API requirements
  ✅ tasks-template.md - Task organization supports independent testing and TDD workflow

Follow-up Items: None

Rationale: Initial version establishing core architectural principles for .NET backend
with EF Core, repository pattern, DI, REST validation, and OpenAPI documentation.
=============================================================================
-->

# .NET Backend API Constitution

## Core Principles

### I. Repository Pattern & Dependency Injection

All data access MUST be encapsulated through the repository pattern with interfaces registered in the dependency injection container. Direct `DbContext` usage in controllers or services is prohibited.

**Requirements:**
- All repositories MUST define interfaces (e.g., `IUserRepository`)
- All repositories MUST be registered in the DI container with appropriate lifetimes
- Controllers and services MUST depend on repository interfaces, never concrete implementations
- Repository implementations MUST handle all EF Core queries and persistence operations

**Rationale:** This architecture ensures testability (repositories can be mocked), maintainability (data access logic is centralized), and adherence to SOLID principles, particularly dependency inversion.

### II. Input Validation (NON-NEGOTIABLE)

ALL incoming REST API requests MUST be validated before processing. No request data may be trusted without explicit validation.

**Requirements:**
- All DTOs/request models MUST use Data Annotations or FluentValidation
- Model validation MUST occur automatically via `[ApiController]` attribute or explicit `ModelState` checks
- Validation failures MUST return HTTP 400 with structured error details
- Custom business rule validations MUST be implemented when annotations are insufficient
- All string inputs MUST be validated for length, format, and content
- All numeric inputs MUST be validated for range and constraints

**Rationale:** Input validation is the first line of defense against injection attacks, data corruption, and system instability. This is non-negotiable for security and reliability.

### III. Entity Framework Core Data Access

Entity Framework Core is the exclusive ORM for all database operations. All data models MUST be configured as EF Core entities with proper migrations.

**Requirements:**
- All entity classes MUST be configured using Fluent API or Data Annotations
- All schema changes MUST be applied through EF Core migrations (never manual SQL)
- All database queries MUST use LINQ or EF Core query methods
- Navigation properties MUST be configured explicitly for relationships
- Eager loading, lazy loading, or explicit loading strategies MUST be chosen deliberately per query
- DbContext MUST be registered with appropriate lifetime (typically Scoped)

**Rationale:** EF Core provides type-safe queries, automatic change tracking, and database abstraction. Consistent usage ensures maintainability and reduces SQL injection risks.

### IV. OpenAPI Documentation (NON-NEGOTIABLE)

Every REST API endpoint MUST be documented using OpenAPI (Swagger) with complete request/response schemas and descriptions.

**Requirements:**
- Swashbuckle or NSwag MUST be configured in the project
- All controllers MUST have XML documentation comments enabled
- All endpoints MUST have `[ProducesResponseType]` attributes for each possible status code
- All DTOs MUST include XML comments describing properties
- API documentation MUST be accessible at `/swagger` in development
- Request/response examples SHOULD be provided for complex operations

**Rationale:** OpenAPI documentation is essential for API consumers, testing, and integration. It serves as the contract between backend and frontend/external systems.

### V. Testing & Quality Assurance

Comprehensive testing MUST cover unit, integration, and contract levels. Test-driven development is strongly encouraged for complex business logic.

**Requirements:**
- All repository implementations MUST have unit tests with mocked DbContext
- All services with business logic MUST have unit tests
- API endpoints SHOULD have integration tests using WebApplicationFactory
- Complex validation logic MUST be independently testable
- Test projects MUST use xUnit, NUnit, or MSTest with appropriate assertion libraries
- Code coverage metrics SHOULD be tracked (target: >80% for critical paths)

**Rationale:** Testing ensures reliability, facilitates refactoring, and documents expected behavior. The testing strategy balances thoroughness with development velocity.

## Technology Stack & Standards

**Framework:** .NET 8 LTS (or current LTS version)  
**ORM:** Entity Framework Core 8.x  
**Validation:** ASP.NET Core Data Annotations + FluentValidation (optional)  
**Documentation:** Swashbuckle.AspNetCore or NSwag  
**Testing:** xUnit + Moq + FluentAssertions (or equivalent)  
**DI Container:** Built-in ASP.NET Core DI container  

**Code Standards:**
- Follow Microsoft C# coding conventions
- Use nullable reference types throughout the project
- Apply `[ApiController]` attribute to all API controllers
- Use async/await for all I/O-bound operations
- Configure logging using `ILogger<T>` injected via DI
- Implement global exception handling middleware

**Prohibited Practices:**
- Direct SQL execution without parameterization (SQL injection risk)
- Synchronous database calls in API endpoints (`ToList()` → use `ToListAsync()`)
- Storing sensitive data in configuration without encryption
- Returning entities directly from controllers (always use DTOs)

## Development Workflow & Quality Gates

**Pre-Implementation:**
1. All feature specifications MUST define acceptance criteria with validation requirements
2. All API contracts MUST be documented before implementation begins
3. Database schema changes MUST be reviewed and approved via migration scripts

**Implementation:**
1. Create EF Core entities and configure relationships
2. Generate and review migration scripts
3. Implement repository interfaces and concrete classes
4. Register repositories in DI container
5. Create request/response DTOs with validation attributes
6. Implement controllers with OpenAPI documentation
7. Write unit tests for repositories and services
8. Write integration tests for critical API endpoints

**Quality Gates:**
- All builds MUST pass without warnings (treat warnings as errors)
- All tests MUST pass before merging
- OpenAPI documentation MUST be validated for completeness
- Code reviews MUST verify: repository pattern usage, input validation, proper async usage, OpenAPI documentation
- Migration scripts MUST be reviewed for production safety (e.g., no data loss)

**Definition of Done:**
- Feature implemented with repository pattern and DI
- All inputs validated with tests proving rejection of invalid data
- OpenAPI documentation complete with examples
- Unit tests passing with adequate coverage
- Integration tests passing for happy path and error cases
- No compiler warnings or code analysis violations

## Governance

This constitution is the authoritative source for architectural decisions and development standards. All code reviews, design discussions, and technical decisions MUST align with these principles.

**Amendment Process:**
- Proposed changes MUST be documented with rationale and impact analysis
- Breaking changes (e.g., replacing EF Core with Dapper) require MAJOR version bump
- New principles or standards require MINOR version bump
- Clarifications and typo fixes require PATCH version bump
- Amendments MUST update this version and last amended date

**Compliance:**
- All pull requests MUST be reviewed for constitutional compliance
- Deviations from principles MUST be explicitly justified and documented
- Repeated violations indicate the constitution needs updating or enforcement improvement
- Architectural Decision Records (ADRs) SHOULD reference relevant constitutional principles

**Versioning:** Follows semantic versioning (MAJOR.MINOR.PATCH)

**Version**: 1.0.0 | **Ratified**: 2025-11-15 | **Last Amended**: 2025-11-15
