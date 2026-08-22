using Week5_TaskTracker_Console.Models;

namespace Week5_TaskTracker_Console.Repositories
{
    public interface ITaskRepository
    {
        Task<int> CreateAsync(TaskItem task);
    }
}
