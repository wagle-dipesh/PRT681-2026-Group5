using Microsoft.Data.Sqlite;

// Simple console CRUD app on a "Tasks" table.
// Uses SQLite for zero-setup local persistence (same relational/CRUD concepts as SQL Server).
// Swapping to SQL Server later only means: 1) change PackageReference to Microsoft.Data.SqlClient,
// 2) change SqliteConnection -> SqlConnection, 3) update the connection string below.

const string connectionString = "Data Source=tasks.db";

InitializeDatabase();

bool running = true;
while (running)
{
    Console.WriteLine("\n--- Tasks CRUD ---");
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
    catch (SqliteException ex)
    {
        // Catch DB-specific errors separately so a locked/corrupt DB gives a
        // useful message instead of an unhandled crash dumping a stack trace.
        Console.WriteLine($"Database error: {ex.Message}");
    }
}

void InitializeDatabase()
{
    using var connection = new SqliteConnection(connectionString);
    connection.Open();
    var createTable = connection.CreateCommand();
    createTable.CommandText =
        @"CREATE TABLE IF NOT EXISTS Tasks (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Title TEXT NOT NULL,
            IsDone INTEGER NOT NULL DEFAULT 0
        );";
    createTable.ExecuteNonQuery();
}

void AddTask()
{
    Console.Write("Task title: ");
    var title = Console.ReadLine();

    // Validate before hitting the database — an empty/whitespace title is a
    // user input error, not a database error, so it's handled here rather
    // than relying on the NOT NULL constraint to reject it.
    if (string.IsNullOrWhiteSpace(title))
    {
        Console.WriteLine("Title cannot be empty. Task not added.");
        return;
    }

    using var connection = new SqliteConnection(connectionString);
    connection.Open();
    var cmd = connection.CreateCommand();
    cmd.CommandText = "INSERT INTO Tasks (Title, IsDone) VALUES ($title, 0);";
    cmd.Parameters.AddWithValue("$title", title.Trim());
    cmd.ExecuteNonQuery();
    Console.WriteLine("Task added.");
}

void ListTasks()
{
    using var connection = new SqliteConnection(connectionString);
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
        var isDone = reader.GetInt32(2) == 1;
        Console.WriteLine($"[{id}] {title} - {(isDone ? "Done" : "Pending")}");
    }

    if (!any)
    {
        // An empty result set isn't an error, but a blank screen looks like
        // one — make the "no data yet" state explicit.
        Console.WriteLine("No tasks yet. Choose option 1 to add one.");
    }
}

void UpdateTask()
{
    Console.Write("Task Id to mark done: ");
    var input = Console.ReadLine();
    if (!int.TryParse(input, out var id))
    {
        Console.WriteLine($"'{input}' is not a valid task Id (must be a whole number).");
        return;
    }

    using var connection = new SqliteConnection(connectionString);
    connection.Open();
    var cmd = connection.CreateCommand();
    cmd.CommandText = "UPDATE Tasks SET IsDone = 1 WHERE Id = $id;";
    cmd.Parameters.AddWithValue("$id", id);
    var rows = cmd.ExecuteNonQuery();
    Console.WriteLine(rows > 0 ? "Task updated." : $"No task found with Id {id}.");
}

void DeleteTask()
{
    Console.Write("Task Id to delete: ");
    var input = Console.ReadLine();
    if (!int.TryParse(input, out var id))
    {
        Console.WriteLine($"'{input}' is not a valid task Id (must be a whole number).");
        return;
    }

    using var connection = new SqliteConnection(connectionString);
    connection.Open();
    var cmd = connection.CreateCommand();
    cmd.CommandText = "DELETE FROM Tasks WHERE Id = $id;";
    cmd.Parameters.AddWithValue("$id", id);
    var rows = cmd.ExecuteNonQuery();
    Console.WriteLine(rows > 0 ? "Task deleted." : $"No task found with Id {id}.");
}
