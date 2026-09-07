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
    public async Task<bool> UpdateTaskAsync(
    int id,
    string title,
    string description,
    bool isCompleted)
{
    const string sql = """
        UPDATE dbo.Tasks
        SET Title = @Title,
            Description = @Description,
            IsCompleted = @IsCompleted
        WHERE Id = @Id;
        """;

    await using var connection =
        new SqlConnection(DatabaseConnection.ConnectionString);

    await using var command = new SqlCommand(sql, connection);

    command.Parameters.AddWithValue("@Id", id);
    command.Parameters.AddWithValue("@Title", title);
    command.Parameters.AddWithValue("@Description", description);
    command.Parameters.AddWithValue("@IsCompleted", isCompleted);

    await connection.OpenAsync();

    var affectedRows = await command.ExecuteNonQueryAsync();

    return affectedRows > 0;
}

public async Task<bool> DeleteTaskAsync(int id)
{
    const string sql = """
        DELETE FROM dbo.Tasks
        WHERE Id = @Id;
        """;

    await using var connection =
        new SqlConnection(DatabaseConnection.ConnectionString);

    await using var command = new SqlCommand(sql, connection);

    command.Parameters.AddWithValue("@Id", id);

    await connection.OpenAsync();

    var affectedRows = await command.ExecuteNonQueryAsync();

    return affectedRows > 0;
}
}