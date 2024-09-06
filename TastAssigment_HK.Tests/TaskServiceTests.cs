using System.Runtime.InteropServices.JavaScript;
using TestAssigment_HK.Models.DTO;
using TestAssigment_HK.Models.Enums;
using TestAssigment_HK.Repositories;
using TestAssigment_HK.Services;
using Task = TestAssigment_HK.Models.Task;
using TaskStatus = TestAssigment_HK.Models.Enums.TaskStatus;
using Microsoft.Extensions.Logging;

namespace TestAssigment_HK.Units;
using Moq;
using Xunit;
public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly TaskService _taskService;

    public TaskServiceTests()
    {
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _taskService = new TaskService(_taskRepositoryMock.Object, Mock.Of<ILogger<TaskService>>());
    }

    [Fact]
    public async System.Threading.Tasks.Task AddTaskSync_Should_Add_Task()
    {
        var taskDTO = new TaskDTO()
        {
            Title = "Test Task",
            Description = "Test Description",
            DueDate = DateTime.UtcNow.AddDays(1),
            Status = TaskStatus.Pending,
            Priority = TaskPriority.Medium
        };
        var userId = Guid.NewGuid();
        await _taskService.AddTaskAsync(taskDTO, userId);
        _taskRepositoryMock.Verify(r=> r.AddTaskAsync(It.IsAny<Task>()), Times.Once);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetFilteredAndSortedTasksAsync_Should_Return_Filtered_Tasks()
    {
        var userId = Guid.NewGuid();
        var tasks = new List<Task>
        {
            new Task
            {
                Id = Guid.NewGuid(), Title = "Task 1", UserId = userId, Status = TaskStatus.Pending,
                Priority = TaskPriority.Low
            },
            new Task
            {
                Id = Guid.NewGuid(), Title = "Task 2", UserId = userId, Status = TaskStatus.InProgress,
                Priority = TaskPriority.High
            }
        };
        _taskRepositoryMock.Setup(r => r.GetTaskByUserIdAsync(userId)).ReturnsAsync(tasks);
        var result =
            await _taskService.GetFilteredAndSortedTasksAsync(userId, TaskStatus.Pending, null, null, "DueDate", false,
                1, 10);
        Assert.Single(result);
        Assert.Equal(TaskStatus.Pending, result.First().Status);
    }
}