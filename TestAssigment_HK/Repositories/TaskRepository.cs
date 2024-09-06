using Microsoft.EntityFrameworkCore;
using TestAssigment_HK.Models;
using Task = TestAssigment_HK.Models.Task;

namespace TestAssigment_HK.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly ApplicationDbContext _context;

    public TaskRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Task>> GetTaskByUserIdAsync(Guid userId)
    {
        return await _context.Tasks.Where(t => t.UserId == userId).ToListAsync();
    }

    public async Task<Task> GetTaskByIdAsync(Guid taskId, Guid userId)
    {
        return await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);
    }

    public async System.Threading.Tasks.Task AddTaskAsync(Task task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
    }

    public async System.Threading.Tasks.Task UpdateTaskAsync(Task task)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
    }

    public async System.Threading.Tasks.Task DeleteTaskAsync(Guid taskId, Guid userId)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);
        if (task is not null)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }
    
}