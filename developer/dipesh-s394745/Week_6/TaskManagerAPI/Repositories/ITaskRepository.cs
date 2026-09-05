using TaskManagerAPI.Models;

namespace TaskManagerAPI.Repositories
{
    public interface ITaskRepository
    {
        Task<List<TaskItem>> GetAllAsync();          
        Task<TaskItem?> GetByIdAsync(int id);        
        Task<TaskItem> CreateAsync(TaskItem task); 
        Task<TaskItem?> UpdateAsync(int id, TaskItem task); 
        Task<bool> DeleteAsync(int id);         
    }
}
