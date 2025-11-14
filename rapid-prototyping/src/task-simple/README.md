# Task Management API

A simple REST API for managing tasks built with Express.js.

## Features

- Create, read, update, and delete tasks
- Filter tasks by status
- In-memory storage
- RESTful endpoints
- Input validation
- Error handling

## Installation

```bash
npm install
```

## Usage

Start the server:
```bash
npm start
```

For development with auto-reload:
```bash
npm run dev
```

The API will be available at `http://localhost:3000`

## API Endpoints

### Get All Tasks
```
GET /api/tasks
```
Query parameters:
- `status` (optional): Filter by status (pending, in-progress, completed)

### Get Task by ID
```
GET /api/tasks/:id
```

### Create New Task
```
POST /api/tasks
```
Body:
```json
{
  "title": "Task title",
  "description": "Task description (optional)",
  "status": "pending (optional, default: pending)"
}
```

### Update Task
```
PUT /api/tasks/:id
```
Body:
```json
{
  "title": "Updated title (optional)",
  "description": "Updated description (optional)",
  "status": "in-progress (optional)"
}
```

### Delete Task
```
DELETE /api/tasks/:id
```

## Task Status Values

- `pending`: Task has not been started
- `in-progress`: Task is currently being worked on
- `completed`: Task has been finished

## Example Usage

Create a task:
```bash
curl -X POST http://localhost:3000/api/tasks \
  -H "Content-Type: application/json" \
  -d '{"title": "Complete project", "description": "Finish the API implementation"}'
```

Get all tasks:
```bash
curl http://localhost:3000/api/tasks
```

Update a task:
```bash
curl -X PUT http://localhost:3000/api/tasks/{task-id} \
  -H "Content-Type: application/json" \
  -d '{"status": "completed"}'
```

Delete a task:
```bash
curl -X DELETE http://localhost:3000/api/tasks/{task-id}
```

## Project Structure

```
task-simple/
├── server.js              # Main server file
├── models/
│   └── Task.js           # Task model
├── controllers/
│   └── taskController.js # Task business logic
├── routes/
│   └── taskRoutes.js     # API routes
├── package.json
└── README.md
```
