using TaskApi.Models.Entities;

namespace TaskApi.Data.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskEntity>> GetAllAsync(string? status = null, string? search = null);
        Task<TaskEntity?> GetByIdAsync(Guid id);
        Task<TaskEntity> CreateAsync(TaskEntity task);
        Task<TaskEntity> UpdateAsync(TaskEntity task);
        Task DeleteAsync(Guid id);
    }
}
