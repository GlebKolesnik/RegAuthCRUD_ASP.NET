using TestAssigment_HK.Models.DTO;
using TestAssigment_HK.Models.Enums;
using TestAssigment_HK.Repositories;
using Task = TestAssigment_HK.Models.Task;
using TaskStatus = TestAssigment_HK.Models.Enums.TaskStatus;

namespace TestAssigment_HK.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ILogger<TaskService> _logger;

    public TaskService(ITaskRepository taskRepository, ILogger<TaskService> logger)
    {
        _taskRepository = taskRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Task>> GetTasksByUserIdAsync(Guid userId)
    {
        return await _taskRepository.GetTaskByUserIdAsync(userId);
    }

    public async Task<Task> GetTaskByIdAsync(Guid taskId, Guid userId)
    {
        return await _taskRepository.GetTaskByIdAsync(taskId, userId);
    }

    public async System.Threading.Tasks.Task AddTaskAsync(TaskDTO taskDto, Guid userId)
    {
        _logger.LogInformation("Creating new task for user {UserId}", userId);
        var task = new Task
        {
            Id = Guid.NewGuid(),
            Title = taskDto.Title,
            Description = taskDto.Description,
            DueDate = taskDto.DueDate?.ToUniversalTime(),
            Status = taskDto.Status,
            Priority = taskDto.Priority,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            UserId = userId
        };
        await _taskRepository.AddTaskAsync(task);
        _logger.LogInformation("Task {TaskID} created successfully for user {UserId}", task.Id, userId);
    }

    public async System.Threading.Tasks.Task UpdateTaskAsync(Guid taskId, TaskDTO taskDto, Guid userId)
    {
        _logger.LogInformation("Updating task {TaskId} for user {UserId}", taskId, userId);
        var task = await _taskRepository.GetTaskByIdAsync(taskId, userId);
        if (task != null)
        {
            task.Title = taskDto.Title;
            task.Description = taskDto.Description;
            task.DueDate = taskDto.DueDate?.ToUniversalTime();
            task.Status = taskDto.Status;
            task.Priority = taskDto.Priority;
            task.UpdatedAt = DateTime.UtcNow;
            await _taskRepository.UpdateTaskAsync(task);
            _logger.LogInformation("Task {TaskId} updated successfully for user {UserId}", taskId, userId);
        }
        else
        {
            _logger.LogWarning("Task {TaskId} not found for user {UserId}", taskId, userId);
        }
    }

    public async System.Threading.Tasks.Task DeleteTaskAsync(Guid taskId, Guid userId)
    {
        _logger.LogInformation("Deleting task {TaskId} for user {UserId}", taskId,userId);
        await _taskRepository.DeleteTaskAsync(taskId, userId);
        _logger.LogInformation("Task {TaskId} was deleted for user {UserId}", taskId, userId);
    }

    public async Task<IEnumerable<Task>> GetFilteredAndSortedTasksAsync(Guid userId, TaskStatus? status,
        DateTime? dueDate, TaskPriority? priority, string sortBy, bool sortDesc, int page, int pageSize)
    {
        _logger.LogInformation("Fetching tasks for user {UserId}", userId);
        var tasks = await _taskRepository.GetTaskByUserIdAsync(userId);
        if (status.HasValue)
        {
            tasks = tasks.Where(t => t.Status == status.Value);
        }

        if (dueDate.HasValue)
        {
            tasks = tasks.Where(t => t.DueDate.HasValue && t.DueDate.Value.Date == dueDate.Value.Date);
        }
        
        if (priority.HasValue)
        {
            tasks = tasks.Where(t => t.Priority == priority.Value);
        }
        //Sorting
        tasks = sortBy switch
        {
            "DueDate" => sortDesc ? tasks.OrderByDescending(t => t.DueDate) : tasks.OrderBy(t => t.DueDate),
            "Priority" => sortDesc ? tasks.OrderByDescending(t => t.Priority) : tasks.OrderBy(t => t.Priority),
            _ => tasks.OrderBy(t => t.DueDate),
        };
        //Pagination
        tasks = tasks.Skip((page - 1) * pageSize).Take(pageSize);
        return tasks;
    }
}