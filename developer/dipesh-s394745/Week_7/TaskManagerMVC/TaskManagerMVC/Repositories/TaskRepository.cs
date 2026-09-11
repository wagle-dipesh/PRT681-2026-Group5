using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using TaskManagerMVC.Data;
using TaskManagerMVC.Exceptions;
using TaskManagerMVC.Models;

namespace TaskManagerMVC.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<TaskRepository> _logger;

        public TaskRepository(IDbConnectionFactory connectionFactory, ILogger<TaskRepository> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<int> CreateTaskAsync(TaskItem task)
        {
            try
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
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error while creating task '{Title}'.", task.Title);
                throw new RepositoryException("Unable to create the task because a database error occurred.", ex);
            }
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
        {
            try
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
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error while retrieving all tasks.");
                throw new RepositoryException("Unable to load the task list because a database error occurred.", ex);
            }
        }

        public async Task<TaskItem?> GetTaskByIdAsync(int id)
        {
            try
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
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error while retrieving task {Id}.", id);
                throw new RepositoryException("Unable to load the task because a database error occurred.", ex);
            }
        }

        public async Task<bool> UpdateTaskAsync(TaskItem task)
        {
            try
            {
                using IDbConnection db = _connectionFactory.CreateConnection();

                var parameters = new DynamicParameters();
                parameters.Add("@Action", "u");
                parameters.Add("@Id", task.Id);
                parameters.Add("@Title", task.Title);
                parameters.Add("@Description", task.Description);
                parameters.Add("@IsCompleted", task.IsCompleted);
                parameters.Add("@RowsAffected", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await db.ExecuteAsync(
                    "spTasks_Manager",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return parameters.Get<int>("@RowsAffected") > 0;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error while updating task {Id}.", task.Id);
                throw new RepositoryException("Unable to update the task because a database error occurred.", ex);
            }
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            try
            {
                using IDbConnection db = _connectionFactory.CreateConnection();

                var parameters = new DynamicParameters();
                parameters.Add("@Action", "d");
                parameters.Add("@Id", id);
                parameters.Add("@RowsAffected", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await db.ExecuteAsync(
                    "spTasks_Manager",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return parameters.Get<int>("@RowsAffected") > 0;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error while deleting task {Id}.", id);
                throw new RepositoryException("Unable to delete the task because a database error occurred.", ex);
            }
        }
    }
}
