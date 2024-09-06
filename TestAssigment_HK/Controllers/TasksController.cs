using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TestAssigment_HK.Models.DTO;
using TestAssigment_HK.Models.Enums;
using TestAssigment_HK.Services;
using TaskStatus = TestAssigment_HK.Models.Enums.TaskStatus;

namespace TestAssigment_HK.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
        private readonly ITaskService _taskService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TasksController(ITaskService taskService, IHttpContextAccessor httpContextAccessor)
        {
                _taskService = taskService;
                _httpContextAccessor = httpContextAccessor;
        }

        private Guid GetCurrentUserId()
        {
                return Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks(
                [FromQuery] TaskStatus? status = null,
                [FromQuery] DateTime? dueDate = null,
                [FromQuery] TaskPriority? priority = null,
                [FromQuery] string sortBy = "DueDate",
                [FromQuery] bool sortDesc = false,
                [FromQuery] int page = 1,
                [FromQuery] int pageSize = 10)
        {
                var userId = GetCurrentUserId();
                var tasks = await _taskService.GetFilteredAndSortedTasksAsync(userId,status,dueDate,priority,sortBy,sortDesc, page,pageSize);
                return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(Guid id)
        {
                var userId = GetCurrentUserId();
                var task = await _taskService.GetTaskByIdAsync(id, userId);
                if (task == null) return NotFound();
                return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskDTO taskDto)
        {
                var userId = GetCurrentUserId();
                await _taskService.AddTaskAsync(taskDto, userId);
                return Ok("Task created successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] TaskDTO taskDto)
        {
                var userId = GetCurrentUserId();
                await _taskService.UpdateTaskAsync(id, taskDto, userId);
                return Ok("Task updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
                var userId = GetCurrentUserId();
                await _taskService.DeleteTaskAsync(id, userId);
                return Ok("Task deleted successfully");
        }
}