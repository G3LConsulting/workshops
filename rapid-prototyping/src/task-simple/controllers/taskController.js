const Task = require('../models/Task');

// In-memory storage for tasks
let tasks = [];

// Get all tasks
const getAllTasks = (req, res) => {
  try {
    const { status } = req.query;
    
    let filteredTasks = tasks;
    if (status) {
      filteredTasks = tasks.filter(task => task.status === status);
    }
    
    res.json({
      success: true,
      count: filteredTasks.length,
      data: filteredTasks
    });
  } catch (error) {
    res.status(500).json({
      success: false,
      error: error.message
    });
  }
};

// Get task by ID
const getTaskById = (req, res) => {
  try {
    const { id } = req.params;
    const task = tasks.find(t => t.id === id);
    
    if (!task) {
      return res.status(404).json({
        success: false,
        error: 'Task not found'
      });
    }
    
    res.json({
      success: true,
      data: task
    });
  } catch (error) {
    res.status(500).json({
      success: false,
      error: error.message
    });
  }
};

// Create new task
const createTask = (req, res) => {
  try {
    const { title, description, status } = req.body;
    
    // Validation
    if (!title || !title.trim()) {
      return res.status(400).json({
        success: false,
        error: 'Title is required'
      });
    }
    
    if (status && !Task.isValidStatus(status)) {
      return res.status(400).json({
        success: false,
        error: 'Invalid status. Must be: pending, in-progress, or completed'
      });
    }
    
    const task = new Task(title.trim(), description?.trim() || '', status);
    tasks.push(task);
    
    res.status(201).json({
      success: true,
      data: task
    });
  } catch (error) {
    res.status(500).json({
      success: false,
      error: error.message
    });
  }
};

// Update task by ID
const updateTask = (req, res) => {
  try {
    const { id } = req.params;
    const { title, description, status } = req.body;
    
    const task = tasks.find(t => t.id === id);
    
    if (!task) {
      return res.status(404).json({
        success: false,
        error: 'Task not found'
      });
    }
    
    // Validation
    if (status && !Task.isValidStatus(status)) {
      return res.status(400).json({
        success: false,
        error: 'Invalid status. Must be: pending, in-progress, or completed'
      });
    }
    
    task.update({ title, description, status });
    
    res.json({
      success: true,
      data: task
    });
  } catch (error) {
    res.status(500).json({
      success: false,
      error: error.message
    });
  }
};

// Delete task by ID
const deleteTask = (req, res) => {
  try {
    const { id } = req.params;
    const taskIndex = tasks.findIndex(t => t.id === id);
    
    if (taskIndex === -1) {
      return res.status(404).json({
        success: false,
        error: 'Task not found'
      });
    }
    
    const deletedTask = tasks.splice(taskIndex, 1)[0];
    
    res.json({
      success: true,
      message: 'Task deleted successfully',
      data: deletedTask
    });
  } catch (error) {
    res.status(500).json({
      success: false,
      error: error.message
    });
  }
};

module.exports = {
  getAllTasks,
  getTaskById,
  createTask,
  updateTask,
  deleteTask
};
