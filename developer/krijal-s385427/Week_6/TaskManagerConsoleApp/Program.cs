using TaskManagerConsoleApp.Services;

var taskService = new TaskService();
var running = true;

while (running)
{
    Console.WriteLine();
    Console.WriteLine("===== TASK MANAGER =====");
    Console.WriteLine("1. Add a task");
    Console.WriteLine("2. View all tasks");
    Console.WriteLine("3. Exit");
    Console.Write("Select an option: ");

    var choice = Console.ReadLine();

    try
    {
        switch (choice)
        {
            case "1":
                Console.Write("Enter task title: ");
                var title = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(title))
                {
                    Console.WriteLine("Task title is required.");
                    break;
                }

                Console.Write("Enter task description: ");
                var description = Console.ReadLine()?.Trim()
                    ?? string.Empty;

                await taskService.AddTaskAsync(title, description);

                Console.WriteLine("Task added successfully.");
                break;

            case "2":
                var tasks = await taskService.GetAllTasksAsync();

                if (tasks.Count == 0)
                {
                    Console.WriteLine("No tasks found.");
                    break;
                }

                Console.WriteLine();
                Console.WriteLine("===== ALL TASKS =====");

                foreach (var task in tasks)
                {
                    var status = task.IsCompleted
                        ? "Completed"
                        : "Pending";

                    Console.WriteLine(
                        $"{task.Id}. {task.Title} | {status}");

                    Console.WriteLine(
                        $"   Description: {task.Description}");

                    Console.WriteLine(
                        $"   Created: {task.CreatedAt:g}");
                }

                break;

            case "3":
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