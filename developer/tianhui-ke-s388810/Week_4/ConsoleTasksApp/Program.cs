using Microsoft.Data.SqlClient;

// Console CRUD app on a "Tasks" table, connecting to a real SQL Server instance
// (running via Docker on this machine — see README.md for the docker run command).
// Server: localhost,1433 — database created automatically on first run if it doesn't exist.

const string masterConnectionString =
    "Server=localhost,1433;Database=master;User Id=sa;Password=Passw0rd123;TrustServerCertificate=True;";
const string appConnectionString =
    "Server=localhost,1433;Database=WeeklyTasksDB;User Id=sa;Password=Passw0rd123;TrustServerCertificate=True;";

InitializeDatabase();

bool running = true;
while (running)
{
    Console.WriteLine("\n--- Tasks CRUD (SQL Server) ---");
    Console.WriteLine("1) Add task  2) List tasks  3) Mark done  4) Delete task  5) Exit");
    Console.Write("Choose: ");
    var choice = Console.ReadLine();

    try
    {
        switch (choice)
        {
            case "1": AddTask(); break;
            case "2": ListTasks(); break;
            case "3": UpdateTask(); break;
            case "4": DeleteTask(); break;
            case "5": running = false; break;
            default: Console.WriteLine("Invalid option, please choose 1-5."); break;
        }
    }
    catch (SqlException ex)
    {
        Console.WriteLine($"Database error: {ex.Message}");
    }
}

void InitializeDatabase()
{
    // Step 1: create the database itself if it doesn't already exist (connect to
    // the built-in 'master' database to do this, since you can't create a database
    // while connected to it).
    using (var masterConnection = new SqlConnection(masterConnectionString))
    {
        masterConnection.Open();
        var createDb = masterConnection.CreateCommand();
        createDb.CommandText = @"
            IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'WeeklyTasksDB')
            BEGIN
                CREATE DATABASE WeeklyTasksDB;
            END";
        createDb.ExecuteNonQuery();
    }

    // Step 2: create the Tasks table inside that database if it doesn't exist yet.
    using var connection = new SqlConnection(appConnectionString);
    connection.Open();
    var createTable = connection.CreateCommand();
    createTable.CommandText = @"
        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Tasks' AND xtype='U')
        BEGIN
            CREATE TABLE Tasks (
                Id INT IDENTITY(1,1) PRIMARY KEY,
                Title NVARCHAR(200) NOT NULL,
                IsDone BIT NOT NULL DEFAULT 0
            );
        END";
    createTable.ExecuteNonQuery();
}

void AddTask()
{
    Console.Write("Task title: ");
    var title = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(title))
    {
        Console.WriteLine("Title cannot be empty. Task not added.");
        return;
    }

    using var connection = new SqlConnection(appConnectionString);
    connection.Open();
    var cmd = connection.CreateCommand();
    cmd.CommandText = "INSERT INTO Tasks (Title, IsDone) VALUES (@Title, 0);";
    cmd.Parameters.AddWithValue("@Title", title.Trim());
    cmd.ExecuteNonQuery();
    Console.WriteLine("Task added.");
}

void ListTasks()
{
    using var connection = new SqlConnection(appConnectionString);
    connection.Open();
    var cmd = connection.CreateCommand();
    cmd.CommandText = "SELECT Id, Title, IsDone FROM Tasks ORDER BY Id;";
    using var reader = cmd.ExecuteReader();

    bool any = false;
    while (reader.Read())
    {
        any = true;
        var id = reader.GetInt32(0);
        var title = reader.GetString(1);
        var isDone = reader.GetBoolean(2);
        Console.WriteLine($"[{id}] {title} - {(isDone ? "Done" : "Pending")}");
    }

    if (!any)
    {
        Console.WriteLine("No tasks yet. Choose option 1 to add one.");
    }
}

void UpdateTask()
{
    Console.Write("Task Id to mark done: ");
    var input = Console.ReadLine();
    if (!int.TryParse(input, out var id))
    {
        Console.WriteLine($"'{input}' is not a valid task Id.");
        return;
    }

    using var connection = new SqlConnection(appConnectionString);
    connection.Open();
    var cmd = connection.CreateCommand();
    cmd.CommandText = "UPDATE Tasks SET IsDone = 1 WHERE Id = @Id;";
    cmd.Parameters.AddWithValue("@Id", id);
    var rows = cmd.ExecuteNonQuery();
    Console.WriteLine(rows > 0 ? "Task updated." : $"No task found with Id {id}.");
}

void DeleteTask()
{
    Console.Write("Task Id to delete: ");
    var input = Console.ReadLine();
    if (!int.TryParse(input, out var id))
    {
        Console.WriteLine($"'{input}' is not a valid task Id.");
        return;
    }

    using var connection = new SqlConnection(appConnectionString);
    connection.Open();
    var cmd = connection.CreateCommand();
    cmd.CommandText = "DELETE FROM Tasks WHERE Id = @Id;";
    cmd.Parameters.AddWithValue("@Id", id);
    var rows = cmd.ExecuteNonQuery();
    Console.WriteLine(rows > 0 ? "Task deleted." : $"No task found with Id {id}.");
}
