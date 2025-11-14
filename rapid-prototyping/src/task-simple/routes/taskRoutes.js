const express = require('express');
const router = express.Router();
const taskController = require('../controllers/taskController');

// Get all tasks
router.get('/', taskController.getAllTasks);

// Get task by ID
router.get('/:id', taskController.getTaskById);

// Create new task
router.post('/', taskController.createTask);

// Update task by ID
router.put('/:id', taskController.updateTask);

// Delete task by ID
router.delete('/:id', taskController.deleteTask);

module.exports = router;
