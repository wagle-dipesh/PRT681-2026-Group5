using Dapper;
using Npgsql;
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

                const string sql = @"
                    INSERT INTO Tasks (Title, Description, IsCompleted, CreatedAt)
                    VALUES (@Title, @Description, @IsCompleted, NOW())
                    RETURNING Id;";

                return await db.ExecuteScalarAsync<int>(sql, new
                {
                    task.Title,
                    task.Description,
                    task.IsCompleted
                });
            }
            catch (NpgsqlException ex)
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

                const string sql = @"
                    SELECT Id, Title, Description, IsCompleted, CreatedAt
                    FROM Tasks
                    ORDER BY CreatedAt DESC;";

                return await db.QueryAsync<TaskItem>(sql);
            }
            catch (NpgsqlException ex)
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

                const string sql = @"
                    SELECT Id, Title, Description, IsCompleted, CreatedAt
                    FROM Tasks
                    WHERE Id = @Id;";

                return await db.QueryFirstOrDefaultAsync<TaskItem>(sql, new { Id = id });
            }
            catch (NpgsqlException ex)
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

                const string sql = @"
                    UPDATE Tasks
                    SET Title = @Title,
                        Description = @Description,
                        IsCompleted = @IsCompleted
                    WHERE Id = @Id;";

                int rowsAffected = await db.ExecuteAsync(sql, new
                {
                    task.Id,
                    task.Title,
                    task.Description,
                    task.IsCompleted
                });

                return rowsAffected > 0;
            }
            catch (NpgsqlException ex)
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

                const string sql = "DELETE FROM Tasks WHERE Id = @Id;";

                int rowsAffected = await db.ExecuteAsync(sql, new { Id = id });

                return rowsAffected > 0;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(ex, "Database error while deleting task {Id}.", id);
                throw new RepositoryException("Unable to delete the task because a database error occurred.", ex);
            }
        }
    }
}
