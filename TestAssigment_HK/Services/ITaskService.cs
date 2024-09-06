using TestAssigment_HK.Models.DTO;
using TestAssigment_HK.Models.Enums;
using Task = TestAssigment_HK.Models.Task;
using TaskStatus = TestAssigment_HK.Models.Enums.TaskStatus;

namespace TestAssigment_HK.Services;

public interface ITaskService
{
    Task<IEnumerable<Task>> GetTasksByUserIdAsync(Guid userId);

    Task<IEnumerable<Models.Task>> GetFilteredAndSortedTasksAsync(Guid userId, TaskStatus? status, DateTime? dueDate,
        TaskPriority? priority, string sortBy, bool sortDesc, int page, int pageSize);
    Task<Task> GetTaskByIdAsync(Guid taskId, Guid userId);
    System.Threading.Tasks.Task AddTaskAsync(TaskDTO taskDto, Guid userId);
    System.Threading.Tasks.Task UpdateTaskAsync(Guid taskId, TaskDTO taskDto, Guid userId);
    System.Threading.Tasks.Task DeleteTaskAsync(Guid taskId, Guid userId);
}