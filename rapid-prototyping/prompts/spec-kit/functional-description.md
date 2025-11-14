# MVP Functional Specification: Task Management API
## Workshop Version - 3 Hour Implementation

## 1. Executive Summary

### 1.1 Purpose
A simplified personal task management API that demonstrates spec-driven development with AI assistance. This MVP focuses on core CRUD operations with authentication, designed to be built within a 3-hour workshop using GitHub Copilot and spec-kit.

### 1.2 Workshop Learning Objectives
- Build a complete REST API using spec-driven development
- Practice AI-assisted coding with GitHub Copilot
- Implement authentication and authorization
- Apply validation and error handling patterns
- Experience the full development cycle from spec to working code

### 1.3 Success Criteria
- Participants complete a working API within workshop timeframe
- All CRUD operations functional with proper authorization
- Input validation and error handling implemented
- API documented with Swagger/OpenAPI

## 2. Domain Model

### 2.1 User Entity
```
User {
  Id: Guid (auto-generated)
  Username: string (unique, required)
  Email: string (unique, required)
  PasswordHash: string (required)
  CreatedAt: DateTime (auto-generated)
}
```

### 2.2 Task Entity
```
Task {
  Id: Guid (auto-generated)
  UserId: Guid (owner, immutable)
  Title: string (required)
  Description: string (optional)
  DueDate: DateTime? (optional)
  Priority: enum (Low, Medium, High)
  Status: enum (Todo, InProgress, Done)
  Tags: string[] (0-5 tags)
  CreatedAt: DateTime (auto-generated)
  UpdatedAt: DateTime (auto-updated)
}
```

## 3. API Endpoints

### 3.1 Authentication Endpoints

#### POST /api/auth/register
Creates a new user account.

**Request Body:**
```json
{
  "username": "johndoe",
  "email": "john@example.com",
  "password": "SecurePass123"
}
```

**Response:** 201 Created
```json
{
  "id": "guid",
  "username": "johndoe",
  "email": "john@example.com",
  "token": "jwt-token"
}
```

#### POST /api/auth/login
Authenticates a user and returns JWT token.

**Request Body:**
```json
{
  "username": "johndoe",
  "password": "SecurePass123"
}
```

**Response:** 200 OK
```json
{
  "token": "jwt-token",
  "username": "johndoe"
}
```

### 3.2 Task Endpoints (Requires Authentication)

#### GET /api/tasks
Returns all tasks for the authenticated user.

**Query Parameters:**
- `status` - Filter by status (Todo, InProgress, Done)
- `search` - Search in title and description

**Response:** 200 OK
```json
[
  {
    "id": "guid",
    "title": "Complete workshop",
    "description": "Build task API with AI assistance",
    "dueDate": "2024-12-01T10:00:00Z",
    "priority": "High",
    "status": "InProgress",
    "tags": ["workshop", "learning"],
    "createdAt": "2024-11-14T09:00:00Z",
    "updatedAt": "2024-11-14T10:30:00Z"
  }
]
```

#### GET /api/tasks/{id}
Returns a specific task by ID.

**Response:** 200 OK or 404 Not Found

#### POST /api/tasks
Creates a new task for the authenticated user.

**Request Body:**
```json
{
  "title": "Complete workshop",
  "description": "Build task API with AI assistance",
  "dueDate": "2024-12-01T10:00:00Z",
  "priority": "High",
  "tags": ["workshop", "learning"]
}
```

**Response:** 201 Created with Location header

#### PUT /api/tasks/{id}
Updates an existing task.

**Request Body:** Same as POST
**Response:** 200 OK or 404 Not Found

#### DELETE /api/tasks/{id}
Deletes a task.

**Response:** 204 No Content or 404 Not Found

#### PATCH /api/tasks/{id}/status
Updates only the status of a task.

**Request Body:**
```json
{
  "status": "Done"
}
```

**Response:** 200 OK

## 4. Validation Rules

### 4.1 User Registration
- **Username**: 3-20 characters, alphanumeric only, case-insensitive unique
- **Email**: Valid email format, unique
- **Password**: Minimum 8 characters, at least one uppercase, one lowercase, one number

### 4.2 Task Validation
- **Title**: Required, 1-200 characters
- **Description**: Optional, max 1000 characters
- **DueDate**: Optional, any valid date/time
- **Priority**: Must be Low, Medium, or High (defaults to Medium)
- **Status**: Must be Todo, InProgress, or Done (defaults to Todo)
- **Tags**: Maximum 5 tags, each max 20 characters

### 4.3 Error Response Format
```json
{
  "type": "ValidationError",
  "title": "One or more validation errors occurred",
  "status": 400,
  "errors": {
    "title": ["Title is required"],
    "priority": ["Invalid priority value"]
  }
}
```

## 5. Business Rules

### 5.1 Authorization Rules
- Users can only view/edit/delete their own tasks
- All task endpoints require valid JWT token
- Tokens expire after 24 hours
- User cannot access another user's tasks (returns 404, not 403)

### 5.2 Task Rules
- New tasks default to "Todo" status and "Medium" priority
- Completed tasks (status = Done) are not deleted, just marked as done
- Tasks without a due date are considered "no deadline"
- Search is case-insensitive and searches both title and description