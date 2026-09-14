using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Models;

namespace TaskManagerApi.Data;

public class TaskManagerContext : DbContext
{
    public TaskManagerContext(DbContextOptions<TaskManagerContext> options)
        : base(options)
    {
    }

    public DbSet<TaskItem> Tasks => Set<TaskItem>();
}