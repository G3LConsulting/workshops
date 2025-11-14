# Task Management API

A comprehensive RESTful API for managing tasks built with .NET 9 and C#. This API provides full CRUD operations with pagination, filtering, and complete OpenAPI documentation.

## Features

- ✅ Create, Read, Update, and Delete tasks
- ✅ Pagination support for listing tasks
- ✅ Filter tasks by status
- ✅ Comprehensive error handling
- ✅ OpenAPI/Swagger documentation
- ✅ In-memory database for development
- ✅ XML documentation for all endpoints

## Technologies Used

- .NET 9
- ASP.NET Core Web API
- Entity Framework Core (In-Memory Database)
- Swashbuckle/Swagger for OpenAPI documentation

## Getting Started

### Prerequisites

- .NET 9 SDK

### Installation

1. Navigate to the project directory:
   ```bash
   cd TaskApi
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Build the project:
   ```bash
   dotnet build
   ```

4. Run the application:
   ```bash
   dotnet run
   ```

5. The API will be available at:
   - HTTP: `http://localhost:5000`
   - HTTPS: `https://localhost:5001`
   - Swagger UI: `https://localhost:5001/` or `http://localhost:5000/`

## API Endpoints

### Base URL
```
http://localhost:5000/api/tasks
```

### Task Status Values
- `Pending` - Task has not been started
- `InProgress` - Task is currently being worked on
- `Completed` - Task has been finished

---

## 1. Get All Tasks (with Pagination and Filtering)

Retrieves a paginated list of tasks with optional filtering by status.

### Endpoint
```
GET /api/tasks
```

### Query Parameters

| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| status | string | No | - | Filter by task status (Pending, InProgress, Completed) |
| pageNumber | integer | No | 1 | Page number (must be > 0) |
| pageSize | integer | No | 10 | Items per page (1-100) |

### Example Request
```bash
# Get all tasks (first page, 10 items)
curl -X GET "http://localhost:5000/api/tasks"

# Get tasks with pagination
curl -X GET "http://localhost:5000/api/tasks?pageNumber=1&pageSize=5"

# Filter by status
curl -X GET "http://localhost:5000/api/tasks?status=Pending"

# Combine filtering and pagination
curl -X GET "http://localhost:5000/api/tasks?status=InProgress&pageNumber=1&pageSize=20"
```

### Example Response (200 OK)
```json
{
  "items": [
    {
      "id": 1,
      "title": "Setup development environment",
      "description": "Install all necessary tools and dependencies",
      "dueDate": "2025-11-16T00:00:00Z",
      "creationDate": "2025-11-09T10:30:00Z",
      "status": "Completed"
    },
    {
      "id": 2,
      "title": "Design database schema",
      "description": "Create entity relationship diagrams and define database structure",
      "dueDate": "2025-11-19T00:00:00Z",
      "creationDate": "2025-11-11T10:30:00Z",
      "status": "InProgress"
    }
  ],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 2,
  "totalPages": 1,
  "hasPreviousPage": false,
  "hasNextPage": false
}
```

### Error Responses

**400 Bad Request** - Invalid pagination parameters
```json
{
  "error": "Page size must be between 1 and 100"
}
```

---

## 2. Get Task by ID

Retrieves a specific task by its ID.

### Endpoint
```
GET /api/tasks/{id}
```

### Path Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| id | integer | Yes | The task ID |

### Example Request
```bash
curl -X GET "http://localhost:5000/api/tasks/1"
```

### Example Response (200 OK)
```json
{
  "id": 1,
  "title": "Setup development environment",
  "description": "Install all necessary tools and dependencies",
  "dueDate": "2025-11-16T00:00:00Z",
  "creationDate": "2025-11-09T10:30:00Z",
  "status": "Completed"
}
```

### Error Responses

**404 Not Found** - Task not found
```json
{
  "error": "Task with ID 99 not found"
}
```

---

## 3. Create a New Task

Creates a new task in the system.

### Endpoint
```
POST /api/tasks
```

### Request Body

| Field | Type | Required | Max Length | Description |
|-------|------|----------|------------|-------------|
| title | string | Yes | 200 | Title of the task |
| description | string | No | 1000 | Detailed description |
| dueDate | datetime | No | - | Due date (ISO 8601 format) |
| status | string | No | - | Initial status (defaults to Pending) |

### Example Request
```bash
curl -X POST "http://localhost:5000/api/tasks" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Implement user authentication",
    "description": "Add JWT-based authentication to the API with role-based authorization",
    "dueDate": "2025-12-01T23:59:59Z",
    "status": "Pending"
  }'
```

### Example Response (201 Created)
```json
{
  "id": 4,
  "title": "Implement user authentication",
  "description": "Add JWT-based authentication to the API with role-based authorization",
  "dueDate": "2025-12-01T23:59:59Z",
  "creationDate": "2025-11-14T10:30:00Z",
  "status": "Pending"
}
```

### Response Headers
```
Location: /api/tasks/4
```

### Error Responses

**400 Bad Request** - Invalid request data
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Title": [
      "Title is required"
    ]
  }
}
```

---

## 4. Update a Task

Updates an existing task by its ID. All fields are optional - only provided fields will be updated.

### Endpoint
```
PUT /api/tasks/{id}
```

### Path Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| id | integer | Yes | The task ID to update |

### Request Body

| Field | Type | Required | Max Length | Description |
|-------|------|----------|------------|-------------|
| title | string | No | 200 | Updated title |
| description | string | No | 1000 | Updated description |
| dueDate | datetime | No | - | Updated due date |
| status | string | No | - | Updated status |

### Example Request
```bash
# Update task status
curl -X PUT "http://localhost:5000/api/tasks/2" \
  -H "Content-Type: application/json" \
  -d '{
    "status": "Completed"
  }'

# Update multiple fields
curl -X PUT "http://localhost:5000/api/tasks/3" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Implement authentication and authorization",
    "description": "Add JWT authentication with role-based access control",
    "status": "InProgress",
    "dueDate": "2025-12-15T23:59:59Z"
  }'
```

### Example Response (200 OK)
```json
{
  "id": 2,
  "title": "Design database schema",
  "description": "Create entity relationship diagrams and define database structure",
  "dueDate": "2025-11-19T00:00:00Z",
  "creationDate": "2025-11-11T10:30:00Z",
  "status": "Completed"
}
```

### Error Responses

**404 Not Found** - Task not found
```json
{
  "error": "Task with ID 99 not found"
}
```

**400 Bad Request** - Invalid data
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Title": [
      "Title cannot exceed 200 characters"
    ]
  }
}
```

---

## 5. Delete a Task

Deletes a task by its ID.

### Endpoint
```
DELETE /api/tasks/{id}
```

### Path Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| id | integer | Yes | The task ID to delete |

### Example Request
```bash
curl -X DELETE "http://localhost:5000/api/tasks/3"
```

### Example Response (204 No Content)
```
No content returned
```

### Error Responses

**404 Not Found** - Task not found
```json
{
  "error": "Task with ID 99 not found"
}
```

---

## HTTP Status Codes

The API uses standard HTTP status codes:

| Status Code | Description |
|-------------|-------------|
| 200 OK | Request succeeded |
| 201 Created | Resource created successfully |
| 204 No Content | Resource deleted successfully |
| 400 Bad Request | Invalid request data or parameters |
| 404 Not Found | Resource not found |
| 500 Internal Server Error | Server error occurred |

---

## Testing with Swagger UI

The easiest way to test the API is through the interactive Swagger UI:

1. Run the application:
   ```bash
   dotnet run
   ```

2. Open your browser and navigate to:
   ```
   http://localhost:5000/
   ```

3. Use the interactive interface to:
   - View all available endpoints
   - See request/response schemas
   - Try out API calls directly in the browser
   - View example requests and responses

---

## Project Structure

```
TaskApi/
├── Controllers/
│   └── TasksController.cs       # API endpoints controller
├── Data/
│   └── TaskDbContext.cs         # Entity Framework DbContext
├── Models/
│   ├── TaskItem.cs              # Task entity model
│   └── DTOs/
│       ├── CreateTaskDto.cs     # DTO for creating tasks
│       ├── UpdateTaskDto.cs     # DTO for updating tasks
│       ├── TaskResponseDto.cs   # DTO for task responses
│       └── PaginatedResponse.cs # Pagination wrapper
├── Program.cs                    # Application entry point
├── EnumSchemaFilter.cs          # Swagger enum configuration
├── TaskApi.csproj               # Project file
└── appsettings.json             # Configuration settings
```

---

## Data Model

### TaskItem

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| Id | int | Yes (auto) | Unique identifier |
| Title | string | Yes | Task title (max 200 chars) |
| Description | string | No | Task description (max 1000 chars) |
| DueDate | DateTime? | No | When the task is due |
| CreationDate | DateTime | Yes (auto) | When the task was created |
| Status | TaskStatus | Yes | Current status (Pending/InProgress/Completed) |

---

## Development Notes

- **Database**: Uses Entity Framework Core In-Memory database for development
- **Seeded Data**: Application includes 3 sample tasks on startup
- **CORS**: Configured to allow all origins in development
- **XML Documentation**: Enabled for comprehensive API documentation
- **Error Handling**: All endpoints include try-catch blocks with appropriate error responses
- **Validation**: Data annotations and model validation throughout

---

## Future Enhancements

Potential improvements for production use:

- [ ] Add persistent database (SQL Server, PostgreSQL, etc.)
- [ ] Implement authentication and authorization
- [ ] Add task assignments and user management
- [ ] Implement task priority levels
- [ ] Add search functionality
- [ ] Include task categories/tags
- [ ] Add due date reminders/notifications
- [ ] Implement task comments/attachments
- [ ] Add audit logging
- [ ] Include unit and integration tests

---

## License

This project is provided as-is for educational and demonstration purposes.

---

## Support

For questions or issues, please refer to the Swagger documentation or check the source code comments.
