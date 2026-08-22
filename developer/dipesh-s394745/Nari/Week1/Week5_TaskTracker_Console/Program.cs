using Week5_TaskTracker_Console.Data;
using Week5_TaskTracker_Console.Models;
using Week5_TaskTracker_Console.Repositories;

string connectionString = "Server=.;Database=TaskTrackerDb;Trusted_Connection=True;TrustServerCertificate=True;";

IDbConnectionFactory connectionFactory = new DbConnectionFactory(connectionString);
ITaskRepository repository = new TaskRepository(connectionFactory);

var task = new TaskItem
{
    Title = "First Task",
    Description = "Testing database insert",
    IsCompleted = false
};

int newId = await repository.CreateAsync(task);
Console.WriteLine($"Inserted Task ID: {newId}");

Console.WriteLine("--- Fetching All Tasks ---");
var tasks = await repository.GetAllAsync();
foreach (var taskList in tasks)
{
    string status = taskList.IsCompleted ? "[DONE]" : "[PENDING]";
    Console.WriteLine($"{taskList.Id}. {status} {taskList.Title} - {taskList.Description}");
}