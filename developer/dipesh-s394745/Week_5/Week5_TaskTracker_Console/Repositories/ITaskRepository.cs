using Week5_TaskTracker_Console.Models;

namespace Week5_TaskTracker_Console.Repositories
{
    public interface ITaskRepository
    {
        Task<int> CreateTaskAsync(TaskItem task);
        Task<IEnumerable<TaskItem>> GetAllTaskAsync();
        Task<TaskItem?> GetTaskByIdAsync(int id);
        Task<bool> UpdateTaskAsync(TaskItem task);
        Task<bool> DeleteTaskAsync(int id);
    }
}
