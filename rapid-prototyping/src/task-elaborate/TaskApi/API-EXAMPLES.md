# API Testing Examples

This document provides various ways to test the Task Management API endpoints.

## Using cURL

### 1. Get All Tasks
```bash
curl -X GET "http://localhost:5000/api/tasks" \
  -H "Accept: application/json"
```

### 2. Get Tasks with Pagination
```bash
curl -X GET "http://localhost:5000/api/tasks?pageNumber=1&pageSize=5" \
  -H "Accept: application/json"
```

### 3. Filter Tasks by Status
```bash
# Get pending tasks
curl -X GET "http://localhost:5000/api/tasks?status=Pending" \
  -H "Accept: application/json"

# Get in-progress tasks
curl -X GET "http://localhost:5000/api/tasks?status=InProgress" \
  -H "Accept: application/json"

# Get completed tasks
curl -X GET "http://localhost:5000/api/tasks?status=Completed" \
  -H "Accept: application/json"
```

### 4. Get Task by ID
```bash
curl -X GET "http://localhost:5000/api/tasks/1" \
  -H "Accept: application/json"
```

### 5. Create a New Task
```bash
curl -X POST "http://localhost:5000/api/tasks" \
  -H "Content-Type: application/json" \
  -H "Accept: application/json" \
  -d '{
    "title": "Write unit tests",
    "description": "Create comprehensive unit tests for all API endpoints",
    "dueDate": "2025-12-01T23:59:59Z",
    "status": "Pending"
  }'
```

### 6. Update a Task
```bash
# Update status only
curl -X PUT "http://localhost:5000/api/tasks/1" \
  -H "Content-Type: application/json" \
  -H "Accept: application/json" \
  -d '{
    "status": "InProgress"
  }'

# Update multiple fields
curl -X PUT "http://localhost:5000/api/tasks/2" \
  -H "Content-Type: application/json" \
  -H "Accept: application/json" \
  -d '{
    "title": "Design database schema - Updated",
    "description": "Create detailed ER diagrams with all relationships",
    "status": "Completed",
    "dueDate": "2025-11-20T23:59:59Z"
  }'
```

### 7. Delete a Task
```bash
curl -X DELETE "http://localhost:5000/api/tasks/1" \
  -H "Accept: application/json"
```

---

## Using PowerShell (Windows)

### 1. Get All Tasks
```powershell
Invoke-RestMethod -Uri "http://localhost:5000/api/tasks" -Method Get
```

### 2. Get Tasks with Filtering
```powershell
Invoke-RestMethod -Uri "http://localhost:5000/api/tasks?status=Pending&pageNumber=1&pageSize=10" -Method Get
```

### 3. Get Task by ID
```powershell
Invoke-RestMethod -Uri "http://localhost:5000/api/tasks/1" -Method Get
```

### 4. Create a New Task
```powershell
$body = @{
    title = "Implement caching"
    description = "Add Redis caching to improve performance"
    dueDate = "2025-12-10T23:59:59Z"
    status = "Pending"
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5000/api/tasks" -Method Post -Body $body -ContentType "application/json"
```

### 5. Update a Task
```powershell
$body = @{
    status = "InProgress"
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5000/api/tasks/2" -Method Put -Body $body -ContentType "application/json"
```

### 6. Delete a Task
```powershell
Invoke-RestMethod -Uri "http://localhost:5000/api/tasks/3" -Method Delete
```

---

## Using HTTPie

### 1. Get All Tasks
```bash
http GET http://localhost:5000/api/tasks
```

### 2. Get Tasks with Filtering
```bash
http GET http://localhost:5000/api/tasks status==Pending pageNumber==1 pageSize==5
```

### 3. Get Task by ID
```bash
http GET http://localhost:5000/api/tasks/1
```

### 4. Create a New Task
```bash
http POST http://localhost:5000/api/tasks \
  title="Deploy to production" \
  description="Deploy the application to production environment" \
  dueDate="2025-12-15T23:59:59Z" \
  status="Pending"
```

### 5. Update a Task
```bash
http PUT http://localhost:5000/api/tasks/2 \
  status="Completed"
```

### 6. Delete a Task
```bash
http DELETE http://localhost:5000/api/tasks/3
```

---

## Using Postman

### Setup
1. Open Postman
2. Set the base URL: `http://localhost:5000/api/tasks`

### 1. Get All Tasks
- **Method:** GET
- **URL:** `http://localhost:5000/api/tasks`
- **Query Params:** (optional)
  - status: Pending
  - pageNumber: 1
  - pageSize: 10

### 2. Get Task by ID
- **Method:** GET
- **URL:** `http://localhost:5000/api/tasks/1`

### 3. Create a New Task
- **Method:** POST
- **URL:** `http://localhost:5000/api/tasks`
- **Headers:**
  - Content-Type: application/json
- **Body (raw JSON):**
```json
{
  "title": "Code review",
  "description": "Review pull requests from the team",
  "dueDate": "2025-11-20T23:59:59Z",
  "status": "Pending"
}
```

### 4. Update a Task
- **Method:** PUT
- **URL:** `http://localhost:5000/api/tasks/2`
- **Headers:**
  - Content-Type: application/json
- **Body (raw JSON):**
```json
{
  "status": "Completed"
}
```

### 5. Delete a Task
- **Method:** DELETE
- **URL:** `http://localhost:5000/api/tasks/3`

---

## Using JavaScript (Fetch API)

### 1. Get All Tasks
```javascript
fetch('http://localhost:5000/api/tasks')
  .then(response => response.json())
  .then(data => console.log(data))
  .catch(error => console.error('Error:', error));
```

### 2. Get Tasks with Filtering
```javascript
const params = new URLSearchParams({
  status: 'Pending',
  pageNumber: 1,
  pageSize: 10
});

fetch(`http://localhost:5000/api/tasks?${params}`)
  .then(response => response.json())
  .then(data => console.log(data))
  .catch(error => console.error('Error:', error));
```

### 3. Create a New Task
```javascript
fetch('http://localhost:5000/api/tasks', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json',
  },
  body: JSON.stringify({
    title: 'Optimize database queries',
    description: 'Analyze and optimize slow running queries',
    dueDate: '2025-12-05T23:59:59Z',
    status: 'Pending'
  })
})
  .then(response => response.json())
  .then(data => console.log('Task created:', data))
  .catch(error => console.error('Error:', error));
```

### 4. Update a Task
```javascript
fetch('http://localhost:5000/api/tasks/2', {
  method: 'PUT',
  headers: {
    'Content-Type': 'application/json',
  },
  body: JSON.stringify({
    status: 'InProgress'
  })
})
  .then(response => response.json())
  .then(data => console.log('Task updated:', data))
  .catch(error => console.error('Error:', error));
```

### 5. Delete a Task
```javascript
fetch('http://localhost:5000/api/tasks/3', {
  method: 'DELETE'
})
  .then(response => {
    if (response.ok) {
      console.log('Task deleted successfully');
    }
  })
  .catch(error => console.error('Error:', error));
```

---

## Using Python (requests library)

### 1. Get All Tasks
```python
import requests

response = requests.get('http://localhost:5000/api/tasks')
print(response.json())
```

### 2. Get Tasks with Filtering
```python
import requests

params = {
    'status': 'Pending',
    'pageNumber': 1,
    'pageSize': 10
}

response = requests.get('http://localhost:5000/api/tasks', params=params)
print(response.json())
```

### 3. Create a New Task
```python
import requests

task_data = {
    'title': 'Update documentation',
    'description': 'Update API documentation with new endpoints',
    'dueDate': '2025-11-25T23:59:59Z',
    'status': 'Pending'
}

response = requests.post('http://localhost:5000/api/tasks', json=task_data)
print(response.json())
```

### 4. Update a Task
```python
import requests

update_data = {
    'status': 'Completed'
}

response = requests.put('http://localhost:5000/api/tasks/2', json=update_data)
print(response.json())
```

### 5. Delete a Task
```python
import requests

response = requests.delete('http://localhost:5000/api/tasks/3')
print(f"Status Code: {response.status_code}")
```

---

## Expected Response Examples

### Success Responses

#### Get All Tasks (200 OK)
```json
{
  "items": [...],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 15,
  "totalPages": 2,
  "hasPreviousPage": false,
  "hasNextPage": true
}
```

#### Get Single Task (200 OK)
```json
{
  "id": 1,
  "title": "Task title",
  "description": "Task description",
  "dueDate": "2025-12-01T23:59:59Z",
  "creationDate": "2025-11-14T10:30:00Z",
  "status": "Pending"
}
```

#### Create Task (201 Created)
```json
{
  "id": 4,
  "title": "New task",
  "description": "Task description",
  "dueDate": "2025-12-01T23:59:59Z",
  "creationDate": "2025-11-14T12:00:00Z",
  "status": "Pending"
}
```

#### Delete Task (204 No Content)
No body returned

### Error Responses

#### Task Not Found (404)
```json
{
  "error": "Task with ID 99 not found"
}
```

#### Validation Error (400)
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Title": ["Title is required"]
  }
}
```

#### Invalid Pagination (400)
```json
{
  "error": "Page size must be between 1 and 100"
}
```

---

## Quick Testing Script (Bash)

Save this as `test-api.sh`:

```bash
#!/bin/bash

BASE_URL="http://localhost:5000/api/tasks"

echo "=== Testing Task Management API ==="
echo

echo "1. Creating a new task..."
TASK_ID=$(curl -s -X POST "$BASE_URL" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Test Task",
    "description": "This is a test task",
    "dueDate": "2025-12-31T23:59:59Z",
    "status": "Pending"
  }' | jq -r '.id')
echo "Created task with ID: $TASK_ID"
echo

echo "2. Getting all tasks..."
curl -s -X GET "$BASE_URL" | jq .
echo

echo "3. Getting task by ID..."
curl -s -X GET "$BASE_URL/$TASK_ID" | jq .
echo

echo "4. Updating task status..."
curl -s -X PUT "$BASE_URL/$TASK_ID" \
  -H "Content-Type: application/json" \
  -d '{
    "status": "InProgress"
  }' | jq .
echo

echo "5. Filtering tasks by status..."
curl -s -X GET "$BASE_URL?status=Pending" | jq .
echo

echo "6. Deleting task..."
curl -s -X DELETE "$BASE_URL/$TASK_ID"
echo "Task deleted"
echo

echo "=== Testing Complete ==="
```

Run with:
```bash
chmod +x test-api.sh
./test-api.sh
```

---

## Notes

- Make sure the API is running (`dotnet run`) before testing
- The API runs on `http://localhost:5000` by default
- All dates should be in ISO 8601 format
- Valid status values: `Pending`, `InProgress`, `Completed`
- For detailed documentation, visit the Swagger UI at `http://localhost:5000/`
