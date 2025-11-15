# Feature Specification: Demo Task API

**Feature Branch**: `001-task-api`  
**Created**: 2025-11-15  
**Status**: Draft  
**Input**: User description: "Demo Task API - A simplified task management API demonstrating core REST principles and spec-driven development with AI assistance"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Create and List Tasks (Priority: P1)

As a workshop participant, I need to create tasks and view all my tasks so that I can track my to-do items through a REST API.

**Why this priority**: This is the core MVP functionality - without the ability to create and list tasks, the API provides no value. This demonstrates basic CRUD operations and serves as the foundation for all other features.

**Independent Test**: Can be fully tested by posting a new task via POST /api/tasks and retrieving it via GET /api/tasks, delivering a working task list.

**Acceptance Scenarios**:

1. **Given** no tasks exist, **When** I POST a task with title "Complete workshop", **Then** I receive a 201 Created response with the task including an auto-generated ID and timestamps
2. **Given** I have created 3 tasks, **When** I GET /api/tasks, **Then** I receive all 3 tasks sorted by creation date (newest first)
3. **Given** I have tasks, **When** I GET /api/tasks?status=complete, **Then** I receive only completed tasks
4. **Given** I have tasks with various titles, **When** I GET /api/tasks?search=workshop, **Then** I receive only tasks containing "workshop" in title or description (case-insensitive)

---

### User Story 2 - View and Update Individual Tasks (Priority: P2)

As a workshop participant, I need to retrieve a specific task by ID and update its details so that I can modify task information as requirements change.

**Why this priority**: After creating tasks, users need to access and modify individual items. This builds on P1 by adding detail view and update capabilities.

**Independent Test**: Can be tested independently by first creating a task (P1), then using GET /api/tasks/{id} to retrieve it and PUT /api/tasks/{id} to update it.

**Acceptance Scenarios**:

1. **Given** a task exists with a specific ID, **When** I GET /api/tasks/{id}, **Then** I receive the full task details with a 200 OK response
2. **Given** a task exists, **When** I PUT /api/tasks/{id} with updated title and description, **Then** the task is updated and returned with a 200 OK response
3. **Given** I request a task with a non-existent ID, **When** I GET /api/tasks/{id}, **Then** I receive a 404 Not Found response with error details
4. **Given** I update a task, **When** the update succeeds, **Then** the UpdatedAt timestamp is automatically refreshed

---

### User Story 3 - Mark Tasks Complete/Incomplete (Priority: P3)

As a workshop participant, I need quick actions to mark tasks as complete or incomplete so that I can track my progress without full updates.

**Why this priority**: This is a convenience feature that simplifies the common action of toggling task status. It demonstrates PATCH operations and provides better UX.

**Independent Test**: Can be tested by creating a task (P1), then using PATCH /api/tasks/{id}/complete and PATCH /api/tasks/{id}/incomplete to toggle status.

**Acceptance Scenarios**:

1. **Given** a task exists with isComplete=false, **When** I PATCH /api/tasks/{id}/complete, **Then** the task's isComplete is set to true and returned with 200 OK
2. **Given** a completed task, **When** I PATCH /api/tasks/{id}/incomplete, **Then** the task's isComplete is set to false and returned with 200 OK
3. **Given** I mark a task complete, **When** the operation succeeds, **Then** the UpdatedAt timestamp is refreshed

---

### User Story 4 - Delete Tasks (Priority: P4)

As a workshop participant, I need to delete tasks that are no longer relevant so that my task list stays clean and manageable.

**Why this priority**: This completes the CRUD operations. While important for completeness, it's the lowest priority since users can work effectively without deletion.

**Independent Test**: Can be tested by creating a task (P1), then using DELETE /api/tasks/{id} to remove it, and verifying with GET /api/tasks that it's gone.

**Acceptance Scenarios**:

1. **Given** a task exists, **When** I DELETE /api/tasks/{id}, **Then** I receive a 204 No Content response
2. **Given** I have deleted a task, **When** I try to GET the deleted task, **Then** I receive a 404 Not Found response
3. **Given** I try to delete a non-existent task, **When** I DELETE /api/tasks/{id}, **Then** I receive a 404 Not Found response

---

### Edge Cases

- What happens when a user submits a task with only whitespace in the title?
- What happens when a user submits a task with a title exceeding 100 characters?
- What happens when a user submits a description exceeding 500 characters?
- How does the system handle invalid date formats for dueDate?
- What happens when searching with an empty search string?
- How does the system handle concurrent updates to the same task?
- What happens when the status query parameter has an invalid value?

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: System MUST provide a REST API endpoint to create new tasks with title, description, and optional due date
- **FR-002**: System MUST auto-generate unique GUIDs for task IDs upon creation
- **FR-003**: System MUST auto-generate CreatedAt and UpdatedAt timestamps for all tasks
- **FR-004**: System MUST provide an endpoint to retrieve all tasks with optional filtering by completion status
- **FR-005**: System MUST provide an endpoint to retrieve a single task by its ID
- **FR-006**: System MUST provide an endpoint to update existing tasks with new title, description, due date, and completion status
- **FR-007**: System MUST provide an endpoint to delete tasks by ID
- **FR-008**: System MUST provide convenience endpoints to mark tasks as complete or incomplete via PATCH operations
- **FR-009**: System MUST support case-insensitive search across task titles and descriptions
- **FR-010**: System MUST validate all incoming request data before processing
- **FR-011**: System MUST return appropriate HTTP status codes (200, 201, 204, 400, 404, 500)
- **FR-012**: System MUST return structured error responses in a consistent format
- **FR-013**: System MUST provide OpenAPI/Swagger documentation for all endpoints
- **FR-014**: System MUST return tasks sorted by creation date (newest first) by default
- **FR-015**: System MUST store all DateTime values in UTC format
- **FR-016**: System MUST set Location header when creating new resources (201 Created)

### Validation Requirements

- **VR-001**: Title MUST be required and cannot be null or empty
- **VR-002**: Title MUST be between 1 and 100 characters
- **VR-003**: Title MUST NOT consist of only whitespace characters
- **VR-004**: Description MAY be null or empty
- **VR-005**: Description MUST NOT exceed 500 characters when provided
- **VR-006**: DueDate MAY be null (optional field)
- **VR-007**: DueDate MUST be a valid DateTime when provided
- **VR-008**: IsComplete MUST default to false for new tasks
- **VR-009**: System MUST return 400 Bad Request with detailed validation errors when validation fails

### Key Entities

- **Task**: Represents a to-do item with the following attributes:
  - Id (Guid): Unique identifier, auto-generated
  - Title (string): Required, 1-100 characters, the task name
  - Description (string): Optional, max 500 characters, detailed task information
  - DueDate (DateTime?): Optional, when the task should be completed
  - IsComplete (bool): Completion status, defaults to false
  - CreatedAt (DateTime): Auto-generated timestamp when task was created
  - UpdatedAt (DateTime): Auto-updated timestamp when task was last modified

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Workshop participants can complete full CRUD implementation in under 45 minutes
- **SC-002**: All 7 API endpoints (GET all, GET by id, POST, PUT, DELETE, PATCH complete, PATCH incomplete) are functional and testable via Swagger UI
- **SC-003**: All validation rules correctly reject invalid input and return clear error messages
- **SC-004**: API documentation is automatically generated and accessible at /swagger endpoint
- **SC-005**: All acceptance scenarios from user stories pass manual testing
- **SC-006**: Filtering and search functionality returns correct results
- **SC-007**: Error responses follow consistent format with appropriate HTTP status codes
- **SC-008**: Timestamps are automatically managed (CreatedAt on creation, UpdatedAt on modification)

### Technical Success Indicators

- Project uses .NET 8 LTS with minimal web API template
- Entity Framework Core In-Memory database for data persistence
- Repository pattern with dependency injection implemented
- FluentValidation or Data Annotations for input validation
- Swashbuckle configured for OpenAPI documentation
- All endpoints follow REST conventions
- Code follows constitution principles (repository pattern, DI, validation, OpenAPI)
