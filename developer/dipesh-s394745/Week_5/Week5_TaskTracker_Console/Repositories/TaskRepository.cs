using Dapper;
using System.Data;
using Week5_TaskTracker_Console.Data;
using Week5_TaskTracker_Console.Models;

namespace Week5_TaskTracker_Console.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public TaskRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task<int> CreateAsync(TaskItem task)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@Action", "i");
            parameters.Add("@Title", task.Title);
            parameters.Add("@Description", task.Description);
            parameters.Add("@IsCompleted", task.IsCompleted);
            parameters.Add("@InsertedId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await db.ExecuteAsync(
                "spTasks_Manager",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return parameters.Get<int>("@InsertedId");
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            using IDbConnection db = _connectionFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@Action", "sa");

            return await db.QueryAsync<TaskItem>(
                "spTasks_Manager",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
