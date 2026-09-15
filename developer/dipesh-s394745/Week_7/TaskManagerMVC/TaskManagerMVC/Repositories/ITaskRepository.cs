using TaskManagerMVC.Models;

namespace TaskManagerMVC.Repositories
{
    public interface ITaskRepository
    {
        Task<int> CreateTaskAsync(TaskItem task);
        Task<IEnumerable<TaskItem>> GetAllTasksAsync();
        Task<TaskItem?> GetTaskByIdAsync(int id);
        Task<bool> UpdateTaskAsync(TaskItem task);
        Task<bool> DeleteTaskAsync(int id);
    }
}
