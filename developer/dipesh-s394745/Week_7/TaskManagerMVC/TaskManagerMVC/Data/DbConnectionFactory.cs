using Microsoft.Data.SqlClient;
using System.Data;

namespace TaskManagerMVC.Data
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public DbConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found in configuration.");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
