# Demo Task API - Functional Specification
## 30-Minute Workshop Exercise Version

## 1. Overview

### 1.1 Purpose
A simplified task management API demonstrating core REST principles and spec-driven development with AI assistance. This demo version focuses exclusively on CRUD operations without authentication, designed to be built in 30-45 minutes as a first hands-on exercise.

### 1.2 Learning Objectives
- Experience spec-driven development workflow
- Practice using GitHub Copilot for code generation
- Implement basic CRUD operations
- Apply input validation
- Generate API documentation with Swagger

### 1.3 Success Criteria
- All CRUD endpoints functional
- Validation returns clear error messages
- API documented and testable via Swagger UI
- Complete implementation in under 45 minutes

## 2. Domain Model

### 2.1 Task Entity
```
Task {
  Id: Guid (auto-generated)
  Title: string (required)
  Description: string (optional)
  DueDate: DateTime? (optional)
  IsComplete: bool (default: false)
  CreatedAt: DateTime (auto-generated)
  UpdatedAt: DateTime (auto-updated)
}
```

## 3. API Endpoints

### 3.1 GET /api/tasks
Returns all tasks, optionally filtered by completion status.

**Query Parameters:**
- `status` (optional): "all" | "complete" | "incomplete" (default: "all")
- `search` (optional): Search text in title and description

**Response:** 200 OK
```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "title": "Complete workshop exercise",
    "description": "Build a task API with AI assistance",
    "dueDate": "2024-12-01T10:00:00Z",
    "isComplete": false,
    "createdAt": "2024-11-14T09:00:00Z",
    "updatedAt": "2024-11-14T09:00:00Z"
  }
]
```

### 3.2 GET /api/tasks/{id}
Returns a single task by ID.

**Response:** 
- 200 OK with task object
- 404 Not Found if task doesn't exist

### 3.3 POST /api/tasks
Creates a new task.

**Request Body:**
```json
{
  "title": "Complete workshop exercise",
  "description": "Build a task API with AI assistance",
  "dueDate": "2024-12-01T10:00:00Z"
}
```

**Response:** 201 Created
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "title": "Complete workshop exercise",
  "description": "Build a task API with AI assistance",
  "dueDate": "2024-12-01T10:00:00Z",
  "isComplete": false,
  "createdAt": "2024-11-14T09:00:00Z",
  "updatedAt": "2024-11-14T09:00:00Z"
}
```

**Headers:**
- `Location`: /api/tasks/{id}

### 3.4 PUT /api/tasks/{id}
Updates an existing task.

**Request Body:**
```json
{
  "title": "Updated task title",
  "description": "Updated description",
  "dueDate": "2024-12-02T10:00:00Z",
  "isComplete": true
}
```

**Response:**
- 200 OK with updated task
- 404 Not Found if task doesn't exist

### 3.5 DELETE /api/tasks/{id}
Deletes a task.

**Response:**
- 204 No Content on success
- 404 Not Found if task doesn't exist

### 3.6 PATCH /api/tasks/{id}/complete
Marks a task as complete.

**Response:**
- 200 OK with updated task
- 404 Not Found if task doesn't exist

### 3.7 PATCH /api/tasks/{id}/incomplete
Marks a task as incomplete.

**Response:**
- 200 OK with updated task
- 404 Not Found if task doesn't exist

## 4. Validation Rules

### 4.1 Task Validation
- **Title**: Required, 1-100 characters, cannot be only whitespace
- **Description**: Optional, maximum 500 characters
- **DueDate**: Optional, any valid DateTime

### 4.2 Error Response Format
All validation errors return 400 Bad Request with this structure:

```json
{
  "type": "ValidationError",
  "title": "One or more validation errors occurred",
  "status": 400,
  "errors": {
    "title": ["Title is required", "Title cannot exceed 100 characters"]
  }
}
```

### 4.3 Standard Error Responses

**404 Not Found:**
```json
{
  "type": "NotFound",
  "title": "Resource not found",
  "status": 404,
  "detail": "Task with id '3fa85f64-5717-4562-b3fc-2c963f66afa6' was not found"
}
```

**500 Internal Server Error:**
```json
{
  "type": "InternalServerError",
  "title": "An error occurred while processing your request",
  "status": 500
}
```

## 5. Business Rules

- New tasks default to `isComplete: false`
- Tasks are returned sorted by creation date (newest first)
- Search is case-insensitive
- Completed tasks remain in the system (soft delete not implemented)
- All DateTime values should be in UTC

