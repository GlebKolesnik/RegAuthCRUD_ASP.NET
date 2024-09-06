using Microsoft.EntityFrameworkCore;
using TestAssigment_HK.Models;
using Task = System.Threading.Tasks.Task;

namespace TestAssigment_HK.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
    public async Task<User> GetUserByEmailOrUsernameAsync(string emailOrUsername)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == emailOrUsername|| u.Username == emailOrUsername);
    }

    public async Task<User> GetUserByIdAsync(Guid userId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task AddUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }
}