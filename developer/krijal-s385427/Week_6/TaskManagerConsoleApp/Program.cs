using Microsoft.Data.SqlClient;
using TaskManagerConsoleApp.Data;

Console.WriteLine("Task Manager Console Application");
Console.WriteLine("Testing SQL Server connection...");

try
{
    await using var connection =
        new SqlConnection(DatabaseConnection.ConnectionString);

    await connection.OpenAsync();

    Console.WriteLine("Successfully connected to TaskManagerDb.");
}
catch (SqlException exception)
{
    Console.WriteLine("Database connection failed.");
    Console.WriteLine(exception.Message);
}