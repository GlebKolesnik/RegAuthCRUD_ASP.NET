using Task = TestAssigment_HK.Models.Task;

namespace TestAssigment_HK.Repositories;

public interface ITaskRepository
{
    Task<IEnumerable<Task>> GetTaskByUserIdAsync(Guid userId);
    Task<Task> GetTaskByIdAsync(Guid taskId, Guid userId);
    System.Threading.Tasks.Task AddTaskAsync(Task task);
    System.Threading.Tasks.Task UpdateTaskAsync(Task task);
    System.Threading.Tasks.Task DeleteTaskAsync(Guid taskId, Guid userId);
}