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

        public async Task<int> CreateTaskAsync(TaskItem task)
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

        public async Task<IEnumerable<TaskItem>> GetAllTaskAsync()
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

        public async Task<TaskItem?> GetTaskByIdAsync(int id)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@Action", "s");
            parameters.Add("@Id", id);

            return await db.QueryFirstOrDefaultAsync<TaskItem>(
                "spTasks_Manager",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<bool> UpdateTaskAsync(TaskItem task)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@Action", "u");
            parameters.Add("@Id", task.Id);
            parameters.Add("@Title", task.Title);
            parameters.Add("@Description", task.Description);
            parameters.Add("@IsCompleted", task.IsCompleted);

            int rows = await db.ExecuteAsync(
                "spTasks_Manager",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return rows > 0;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@Action", "d");
            parameters.Add("@Id", id);

            int rows = await db.ExecuteAsync(
                "spTasks_Manager",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return rows > 0;
        }
    }
}
