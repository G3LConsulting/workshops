const { v4: uuidv4 } = require('uuid');

class Task {
  constructor(title, description, status = 'pending') {
    this.id = uuidv4();
    this.title = title;
    this.description = description;
    this.status = status; // pending, in-progress, completed
    this.createdAt = new Date().toISOString();
    this.updatedAt = new Date().toISOString();
  }

  update(updates) {
    if (updates.title !== undefined) this.title = updates.title;
    if (updates.description !== undefined) this.description = updates.description;
    if (updates.status !== undefined) this.status = updates.status;
    this.updatedAt = new Date().toISOString();
  }

  static isValidStatus(status) {
    return ['pending', 'in-progress', 'completed'].includes(status);
  }
}

module.exports = Task;
