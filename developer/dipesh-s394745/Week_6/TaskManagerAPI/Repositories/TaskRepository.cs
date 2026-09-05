using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        // Get ALL tasks from the database
        public async Task<List<TaskItem>> GetAllAsync()
        {
            return await _context.TaskItems.ToListAsync();
        }

        // Get ONE task by its ID
        public async Task<TaskItem?> GetByIdAsync(int id)
        {
            return await _context.TaskItems.FindAsync(id);
        }

        // Create a NEW task and save it to the database
        public async Task<TaskItem> CreateAsync(TaskItem task)
        {
            _context.TaskItems.Add(task);       
            await _context.SaveChangesAsync();  
            return task;                         
        }

        // UPDATE an existing task
        public async Task<TaskItem?> UpdateAsync(int id, TaskItem updatedTask)
        {
            var existingTask = await _context.TaskItems.FindAsync(id);

            if (existingTask == null)
                return null;  

            // Update the fields
            existingTask.Title = updatedTask.Title;
            existingTask.Description = updatedTask.Description;
            existingTask.IsCompleted = updatedTask.IsCompleted;

            await _context.SaveChangesAsync();
            return existingTask;
        }

        // DELETE a task by ID
        public async Task<bool> DeleteAsync(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);

            if (task == null)
                return false; 

            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();
            return true;  
        }
    }
}
