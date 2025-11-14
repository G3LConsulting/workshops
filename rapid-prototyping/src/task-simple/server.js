const express = require('express');
const bodyParser = require('body-parser');
const taskRoutes = require('./routes/taskRoutes');

const app = express();
const PORT = process.env.PORT || 3000;

// Middleware
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true }));

// Routes
app.use('/api/tasks', taskRoutes);

// Root endpoint
app.get('/', (req, res) => {
  res.json({
    message: 'Task Management API',
    endpoints: {
      'GET /api/tasks': 'Get all tasks',
      'GET /api/tasks/:id': 'Get task by ID',
      'POST /api/tasks': 'Create new task',
      'PUT /api/tasks/:id': 'Update task by ID',
      'DELETE /api/tasks/:id': 'Delete task by ID'
    }
  });
});

// Error handling middleware
app.use((err, req, res, next) => {
  console.error(err.stack);
  res.status(500).json({ error: 'Something went wrong!' });
});

// 404 handler
app.use((req, res) => {
  res.status(404).json({ error: 'Route not found' });
});

app.listen(PORT, () => {
  console.log(`Task Management API running on port ${PORT}`);
});

module.exports = app;
