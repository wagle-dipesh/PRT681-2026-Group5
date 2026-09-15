using Microsoft.AspNetCore.Mvc;
using TaskManagerMVC.Exceptions;
using TaskManagerMVC.Models;
using TaskManagerMVC.Repositories;

namespace TaskManagerMVC.Controllers
{
    public class TasksController : Controller
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ITaskRepository taskRepository, ILogger<TasksController> logger)
        {
            _taskRepository = taskRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var model = new TasksIndexViewModel
            {
                SuccessMessage = TempData["SuccessMessage"] as string,
                ErrorMessage = TempData["ErrorMessage"] as string
            };

            try
            {
                model.Tasks = await _taskRepository.GetAllTasksAsync();
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, "Failed to load task list.");
                model.ErrorMessage = ex.Message;
            }

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var task = await _taskRepository.GetTaskByIdAsync(id);
                if (task == null)
                {
                    TempData["ErrorMessage"] = $"Task with ID {id} was not found.";
                    return RedirectToAction(nameof(Index));
                }

                return View(task);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, "Failed to load task {Id}.", id);
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Create()
        {
            return View(new TaskItem());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,IsCompleted")] TaskItem task)
        {
            if (!ModelState.IsValid)
            {
                return View(task);
            }

            try
            {
                task.CreatedAt = DateTime.Now;
                await _taskRepository.CreateTaskAsync(task);
                TempData["SuccessMessage"] = "Task created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, "Failed to create task '{Title}'.", task.Title);
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(task);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var task = await _taskRepository.GetTaskByIdAsync(id);
                if (task == null)
                {
                    TempData["ErrorMessage"] = $"Task with ID {id} was not found.";
                    return RedirectToAction(nameof(Index));
                }

                return View(task);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, "Failed to load task {Id} for editing.", id);
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,IsCompleted,CreatedAt")] TaskItem task)
        {
            if (id != task.Id)
            {
                TempData["ErrorMessage"] = "Task ID mismatch.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                return View(task);
            }

            try
            {
                bool updated = await _taskRepository.UpdateTaskAsync(task);
                if (!updated)
                {
                    TempData["ErrorMessage"] = $"Task with ID {id} was not found.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = "Task updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, "Failed to update task {Id}.", id);
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(task);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var task = await _taskRepository.GetTaskByIdAsync(id);
                if (task == null)
                {
                    TempData["ErrorMessage"] = $"Task with ID {id} was not found.";
                    return RedirectToAction(nameof(Index));
                }

                return View(task);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, "Failed to load task {Id} for deletion.", id);
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                bool deleted = await _taskRepository.DeleteTaskAsync(id);
                TempData[deleted ? "SuccessMessage" : "ErrorMessage"] =
                    deleted ? "Task deleted successfully." : $"Task with ID {id} was not found.";
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, "Failed to delete task {Id}.", id);
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
