using Microsoft.EntityFrameworkCore;

namespace TaskApi.Data;

/// <summary>
/// Database context for the Task Management API
/// </summary>
public class TaskDbContext : DbContext
{
    public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// DbSet for TaskItem entities
    /// </summary>
    public DbSet<Models.TaskItem> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure TaskItem entity
        modelBuilder.Entity<Models.TaskItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.CreationDate).IsRequired();
            entity.Property(e => e.Status).IsRequired();
        });

        // Seed some initial data for development
        modelBuilder.Entity<Models.TaskItem>().HasData(
            new Models.TaskItem
            {
                Id = 1,
                Title = "Setup development environment",
                Description = "Install all necessary tools and dependencies",
                CreationDate = DateTime.UtcNow.AddDays(-5),
                DueDate = DateTime.UtcNow.AddDays(2),
                Status = Models.TaskStatus.Completed
            },
            new Models.TaskItem
            {
                Id = 2,
                Title = "Design database schema",
                Description = "Create entity relationship diagrams and define database structure",
                CreationDate = DateTime.UtcNow.AddDays(-3),
                DueDate = DateTime.UtcNow.AddDays(5),
                Status = Models.TaskStatus.InProgress
            },
            new Models.TaskItem
            {
                Id = 3,
                Title = "Implement authentication",
                Description = "Add JWT-based authentication to the API",
                CreationDate = DateTime.UtcNow.AddDays(-1),
                DueDate = DateTime.UtcNow.AddDays(10),
                Status = Models.TaskStatus.Pending
            }
        );
    }
}
