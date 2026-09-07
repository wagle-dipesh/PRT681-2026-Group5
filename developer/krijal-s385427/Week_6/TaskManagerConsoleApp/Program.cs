using TaskManagerConsoleApp.Services;

var taskService = new TaskService();
var running = true;

while (running)
{
    Console.WriteLine();
    Console.WriteLine("===== TASK MANAGER =====");
    Console.WriteLine("1. Add a task");
    Console.WriteLine("2. View all tasks");
    Console.WriteLine("3. Update a task");
    Console.WriteLine("4. Delete a task");
    Console.WriteLine("5. Exit");
    Console.Write("Select an option: ");

    var choice = Console.ReadLine();

    try
    {
        switch (choice)
        {
            case "1":
                await AddTaskAsync();
                break;

            case "2":
                await ViewTasksAsync();
                break;

            case "3":
                await UpdateTaskAsync();
                break;

            case "4":
                await DeleteTaskAsync();
                break;

            case "5":
                running = false;
                Console.WriteLine("Application closed.");
                break;

            default:
                Console.WriteLine("Please select a valid option.");
                break;
        }
    }
    catch (Exception exception)
    {
        Console.WriteLine($"An error occurred: {exception.Message}");
    }
}

async Task AddTaskAsync()
{
    Console.Write("Enter task title: ");
    var title = Console.ReadLine()?.Trim();

    if (string.IsNullOrWhiteSpace(title))
    {
        Console.WriteLine("Task title is required.");
        return;
    }

    Console.Write("Enter task description: ");
    var description = Console.ReadLine()?.Trim() ?? string.Empty;

    await taskService.AddTaskAsync(title, description);

    Console.WriteLine("Task added successfully.");
}

async Task ViewTasksAsync()
{
    var tasks = await taskService.GetAllTasksAsync();

    if (tasks.Count == 0)
    {
        Console.WriteLine("No tasks found.");
        return;
    }

    Console.WriteLine();
    Console.WriteLine("===== ALL TASKS =====");

    foreach (var task in tasks)
    {
        var status = task.IsCompleted ? "Completed" : "Pending";

        Console.WriteLine($"{task.Id}. {task.Title} | {status}");
        Console.WriteLine($"   Description: {task.Description}");
        Console.WriteLine($"   Created: {task.CreatedAt:g}");
    }
}

async Task UpdateTaskAsync()
{
    Console.Write("Enter the task ID to update: ");

    if (!int.TryParse(Console.ReadLine(), out var id))
    {
        Console.WriteLine("Please enter a valid numeric ID.");
        return;
    }

    Console.Write("Enter the new title: ");
    var title = Console.ReadLine()?.Trim();

    if (string.IsNullOrWhiteSpace(title))
    {
        Console.WriteLine("Task title is required.");
        return;
    }

    Console.Write("Enter the new description: ");
    var description = Console.ReadLine()?.Trim() ?? string.Empty;

    Console.Write("Is the task completed? (y/n): ");
    var statusInput = Console.ReadLine()?.Trim().ToLower();

    if (statusInput is not ("y" or "n"))
    {
        Console.WriteLine("Please enter y or n.");
        return;
    }

    var isCompleted = statusInput == "y";

    var updated = await taskService.UpdateTaskAsync(
        id,
        title,
        description,
        isCompleted);

    Console.WriteLine(
        updated
            ? "Task updated successfully."
            : "Task not found.");
}

async Task DeleteTaskAsync()
{
    Console.Write("Enter the task ID to delete: ");

    if (!int.TryParse(Console.ReadLine(), out var id))
    {
        Console.WriteLine("Please enter a valid numeric ID.");
        return;
    }

    Console.Write($"Delete task {id}? (y/n): ");
    var confirmation = Console.ReadLine()?.Trim().ToLower();

    if (confirmation != "y")
    {
        Console.WriteLine("Delete cancelled.");
        return;
    }

    var deleted = await taskService.DeleteTaskAsync(id);

    Console.WriteLine(
        deleted
            ? "Task deleted successfully."
            : "Task not found.");
}