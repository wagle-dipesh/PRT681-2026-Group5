using Week5_TaskTracker_Console.Data;
using Week5_TaskTracker_Console.Models;
using Week5_TaskTracker_Console.Repositories;

string connectionString = "Server=.;Database=TaskTrackerDb;Trusted_Connection=True;TrustServerCertificate=True;";
IDbConnectionFactory connectionFactory = new DbConnectionFactory(connectionString);
ITaskRepository repository = new TaskRepository(connectionFactory);

bool running = true;

while (running)
{
    Console.WriteLine();
    Console.WriteLine("=== Task Tracker ===");
    Console.WriteLine("1. View all tasks");
    Console.WriteLine("2. View task by ID");
    Console.WriteLine("3. Add new task");
    Console.WriteLine("4. Update a task");
    Console.WriteLine("5. Delete a task");
    Console.WriteLine("6. Exit");
    Console.Write("Your choice: ");

    string? choice = Console.ReadLine();

    if (choice == "1")
    {
        var tasks = await repository.GetAllTaskAsync();
        var list = tasks.ToList();

        if (list.Count == 0)
        {
            Console.WriteLine("No tasks found.");
        }
        else
        {
            Console.WriteLine();
            foreach (var t in list)
            {
                string status = t.IsCompleted ? "Done" : "Pending";
                Console.WriteLine($"[{t.Id}] {t.Title} - {t.Description} ({status}) - Created: {t.CreatedAt:dd MMM yyyy}");
            }
        }
    }

    else if (choice == "2")
    {
        Console.Write("Enter task ID: ");
        int id = int.Parse(Console.ReadLine()!);

        var task = await repository.GetTaskByIdAsync(id);

        if (task == null)
        {
            Console.WriteLine("Task not found.");
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("ID          : " + task.Id);
            Console.WriteLine("Title       : " + task.Title);
            Console.WriteLine("Description : " + task.Description);
            Console.WriteLine("Status      : " + (task.IsCompleted ? "Done" : "Pending"));
            Console.WriteLine("Created     : " + task.CreatedAt.ToString("dd MMM yyyy HH:mm"));
        }
    }

    else if (choice == "3")
    {
        Console.Write("Title: ");
        string title = Console.ReadLine()!;

        Console.Write("Description: ");
        string description = Console.ReadLine()!;

        Console.Write("Is it already completed? (y/n): ");
        bool isCompleted = Console.ReadLine()!.Trim().ToLower() == "y";

        var newTask = new TaskItem
        {
            Title = title,
            Description = description,
            IsCompleted = isCompleted
        };

        int newId = await repository.CreateTaskAsync(newTask);
        Console.WriteLine("Task added! ID: " + newId);
    }

    else if (choice == "4")
    {
        Console.Write("Enter task ID to update: ");
        int id = int.Parse(Console.ReadLine()!);

        var task = await repository.GetTaskByIdAsync(id);

        if (task == null)
        {
            Console.WriteLine("Task not found.");
        }
        else
        {
            Console.WriteLine("Current title: " + task.Title);
            Console.Write("New title (press Enter to keep): ");
            string newTitle = Console.ReadLine()!;

            Console.WriteLine("Current description: " + task.Description);
            Console.Write("New description (press Enter to keep): ");
            string newDesc = Console.ReadLine()!;

            Console.WriteLine("Current status: " + (task.IsCompleted ? "Done" : "Pending"));
            Console.Write("Mark as completed? (y/n, press Enter to keep): ");
            string statusInput = Console.ReadLine()!.Trim().ToLower();

            task.Title       = string.IsNullOrEmpty(newTitle) ? task.Title : newTitle;
            task.Description = string.IsNullOrEmpty(newDesc)  ? task.Description : newDesc;

            if (statusInput == "y") task.IsCompleted = true;
            else if (statusInput == "n") task.IsCompleted = false;

            bool updated = await repository.UpdateTaskAsync(task);
            Console.WriteLine(updated ? "Task updated successfully." : "Update failed.");
        }
    }

    else if (choice == "5")
    {
        Console.Write("Enter task ID to delete: ");
        int id = int.Parse(Console.ReadLine()!);

        Console.Write("Are you sure? (y/n): ");
        string confirm = Console.ReadLine()!.Trim().ToLower();

        if (confirm == "y")
        {
            bool deleted = await repository.DeleteTaskAsync(id);
            Console.WriteLine(deleted ? "Task deleted." : "Task not found.");
        }
        else
        {
            Console.WriteLine("Cancelled.");
        }
    }

    else if (choice == "6")
    {
        Console.WriteLine("Goodbye!");
        running = false;
    }

    else
    {
        Console.WriteLine("Please enter a number between 1 and 6.");
    }
}

