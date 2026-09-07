using Microsoft.Data.SqlClient;
using TaskManagerConsoleApp.Data;
using TaskManagerConsoleApp.Models;

namespace TaskManagerConsoleApp.Services;

public class TaskService
{
    public async Task AddTaskAsync(string title, string description)
    {
        const string sql = """
            INSERT INTO dbo.Tasks (Title, Description)
            VALUES (@Title, @Description);
            """;

        await using var connection =
            new SqlConnection(DatabaseConnection.ConnectionString);

        await using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@Title", title);
        command.Parameters.AddWithValue("@Description", description);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task<List<TaskItem>> GetAllTasksAsync()
    {
        const string sql = """
            SELECT Id, Title, Description, IsCompleted, CreatedAt
            FROM dbo.Tasks
            ORDER BY Id;
            """;

        var tasks = new List<TaskItem>();

        await using var connection =
            new SqlConnection(DatabaseConnection.ConnectionString);

        await using var command = new SqlCommand(sql, connection);

        await connection.OpenAsync();

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            tasks.Add(new TaskItem
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Description = reader.IsDBNull(2)
                    ? string.Empty
                    : reader.GetString(2),
                IsCompleted = reader.GetBoolean(3),
                CreatedAt = reader.GetDateTime(4)
            });
        }

        return tasks;
    }
}