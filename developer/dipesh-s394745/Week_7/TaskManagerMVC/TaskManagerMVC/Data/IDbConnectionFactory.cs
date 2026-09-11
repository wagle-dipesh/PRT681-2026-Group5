using System.Data;

namespace TaskManagerMVC.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
