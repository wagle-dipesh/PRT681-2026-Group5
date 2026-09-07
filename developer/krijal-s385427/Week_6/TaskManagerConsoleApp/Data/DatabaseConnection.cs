namespace TaskManagerConsoleApp.Data;

public static class DatabaseConnection
{
    public const string ConnectionString =
        @"Server=.\SQLEXPRESS;
          Database=TaskManagerDb;
          Trusted_Connection=True;
          TrustServerCertificate=True;";
}