using Microsoft.EntityFrameworkCore;
using TaskApi.Models.Entities;

namespace TaskApi.Data.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskDbContext _context;

        public TaskRepository(TaskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskEntity>> GetAllAsync(string? status = null, string? search = null)
        {
            IQueryable<TaskEntity> query = _context.Tasks;

            if (status == "complete") query = query.Where(t => t.IsComplete);
            if (status == "incomplete") query = query.Where(t => !t.IsComplete);
            if (!string.IsNullOrEmpty(search))
                query = query.Where(t => t.Title.Contains(search) || 
                                       (t.Description != null && t.Description.Contains(search)));

            return await query.OrderByDescending(t => t.CreatedAt).ToListAsync();
        }

        public async Task<TaskEntity?> GetByIdAsync(Guid id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task<TaskEntity> CreateAsync(TaskEntity task)
        {
            task.Id = Guid.NewGuid();
            task.CreatedAt = DateTime.UtcNow;
            task.UpdatedAt = DateTime.UtcNow;
            task.IsComplete = false;

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<TaskEntity> UpdateAsync(TaskEntity task)
        {
            task.UpdatedAt = DateTime.UtcNow;
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task DeleteAsync(Guid id)
        {
            var task = await GetByIdAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }
    }
}
