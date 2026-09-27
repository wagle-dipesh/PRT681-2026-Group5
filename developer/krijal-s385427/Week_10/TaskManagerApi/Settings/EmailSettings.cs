namespace TaskManagerApi.Settings;

public class EmailSettings
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 1025;
    public bool UseSsl { get; set; }
    public string FromName { get; set; } = "Task Manager";
    public string FromAddress { get; set; } = "taskmanager@example.local";
    public string? Username { get; set; }
    public string? Password { get; set; }
}