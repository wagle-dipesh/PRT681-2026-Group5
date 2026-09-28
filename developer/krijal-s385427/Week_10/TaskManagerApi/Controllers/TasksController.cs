using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Data;
using TaskManagerApi.Models;

namespace TaskManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly TaskManagerContext _context;

    public TasksController(TaskManagerContext context)
    {
        _context = context;
    }

    // GET: api/tasks
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
    {
        var tasks = await _context.Tasks
            .OrderByDescending(task => task.CreatedAt)
            .ToListAsync();

        return Ok(tasks);
    }

    // GET: api/tasks/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskItem>> GetTask(int id)
    {
        var task = await _context.Tasks.FindAsync(id);

        if (task is null)
        {
            return NotFound(new
            {
                message = $"Task with ID {id} was not found."
            });
        }

        return Ok(task);
    }

    // POST: api/tasks
    [HttpPost]
    public async Task<ActionResult<TaskItem>> CreateTask(TaskItem task)
    {
        task.Id = 0;
        task.CreatedAt = DateTime.Now;

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetTask),
            new { id = task.Id },
            task);
    }

    // PUT: api/tasks/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateTask(int id, TaskItem updatedTask)
    {
        var existingTask = await _context.Tasks.FindAsync(id);

        if (existingTask is null)
        {
            return NotFound(new
            {
                message = $"Task with ID {id} was not found."
            });
        }

        existingTask.Title = updatedTask.Title;
        existingTask.Description = updatedTask.Description;
        existingTask.IsCompleted = updatedTask.IsCompleted;

        await _context.SaveChangesAsync();

        return Ok(existingTask);
    }

    // DELETE: api/tasks/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await _context.Tasks.FindAsync(id);

        if (task is null)
        {
            return NotFound(new
            {
                message = $"Task with ID {id} was not found."
            });
        }

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}