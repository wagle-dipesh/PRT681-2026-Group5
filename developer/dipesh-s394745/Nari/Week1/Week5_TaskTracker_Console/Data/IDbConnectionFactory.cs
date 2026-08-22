using System.Data;

namespace Week5_TaskTracker_Console.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();

    }
}
