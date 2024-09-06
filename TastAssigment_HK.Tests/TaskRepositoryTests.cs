using Microsoft.EntityFrameworkCore;
using TestAssigment_HK.Models;
using TestAssigment_HK.Repositories;
using Task = TestAssigment_HK.Models.Task;

namespace TestAssigment_HK.Units;

public class TaskRepositoryTests
{
    private readonly TaskRepository _taskRepository;
    private readonly ApplicationDbContext _context;

    public TaskRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _context = new ApplicationDbContext(options);
        _taskRepository = new TaskRepository(_context);
    }

    [Fact]
    public async System.Threading.Tasks.Task AddTask_Should_Add_Task()
    {
        var task = new Task()
        {
            Id = Guid.NewGuid(),
            Title = "Test Task",
            Description = "Test Description",
            UserId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await _taskRepository.AddTaskAsync(task);
        var result = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == task.Id);
        Assert.NotNull(result);
        Assert.Equal("Test Task", result.Title);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetTasksByUserIdAsync_Should_Return_Tasks_For_User()
    {
        var userId = Guid.NewGuid();
        var task1 = new Task { Id = Guid.NewGuid(), Title = "Task 1",Description = "Description 1",UserId = userId };
        var task2 = new Task { Id = Guid.NewGuid(), Title = "Task 2",Description = "Description 2",UserId = userId };
        _context.Tasks.AddRange(task1, task2);
        await _context.SaveChangesAsync();
        var result = await _taskRepository.GetTaskByUserIdAsync(userId);
        Assert.Equal(2, result.Count());
    }
}