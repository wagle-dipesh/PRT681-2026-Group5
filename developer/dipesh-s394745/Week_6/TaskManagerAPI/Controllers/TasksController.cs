using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.DTOs;
using TaskManagerAPI.Models;
using TaskManagerAPI.Repositories;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _repository;

        public TasksController(ITaskRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var tasks = await _repository.GetAllAsync();

                var response = tasks.Select(t => new TaskResponseDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                    CreatedAt = t.CreatedAt
                });

                return Ok(response);  
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Something went wrong", error = ex.Message });
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var task = await _repository.GetByIdAsync(id);

                if (task == null)
                    return NotFound(new { message = $"Task with ID {id} was not found" });  // 404

                var response = new TaskResponseDto
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    IsCompleted = task.IsCompleted,
                    CreatedAt = task.CreatedAt
                };

                return Ok(response);  
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Something went wrong", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);  

                var newTask = new TaskItem
                {
                    Title = dto.Title,
                    Description = dto.Description
                };

                var created = await _repository.CreateAsync(newTask);

                var response = new TaskResponseDto
                {
                    Id = created.Id,
                    Title = created.Title,
                    Description = created.Description,
                    IsCompleted = created.IsCompleted,
                    CreatedAt = created.CreatedAt
                };

                return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Something went wrong", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);  

                var updatedTask = new TaskItem
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    IsCompleted = dto.IsCompleted
                };

                var result = await _repository.UpdateAsync(id, updatedTask);

                if (result == null)
                    return NotFound(new { message = $"Task with ID {id} was not found" }); 

                var response = new TaskResponseDto
                {
                    Id = result.Id,
                    Title = result.Title,
                    Description = result.Description,
                    IsCompleted = result.IsCompleted,
                    CreatedAt = result.CreatedAt
                };

                return Ok(response);  
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Something went wrong", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _repository.DeleteAsync(id);

                if (!deleted)
                    return NotFound(new { message = $"Task with ID {id} was not found" });  

                return NoContent();  
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Something went wrong", error = ex.Message });
            }
        }
    }
}
