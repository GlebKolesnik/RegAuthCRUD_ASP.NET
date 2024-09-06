using Microsoft.EntityFrameworkCore;
using TestAssigment_HK.Models;
using TestAssigment_HK.Repositories;

namespace TestAssigment_HK.Units;

public class UserRepositoryTests
{
    private readonly UserRepository _userRepository;
    private readonly ApplicationDbContext _context;

    public UserRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _context = new ApplicationDbContext(options);
        _userRepository = new UserRepository(_context);
    }

    [Fact]
    public async System.Threading.Tasks.Task AddUserAsync_Should_Add_User()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "hashedpassword",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await _userRepository.AddUserAsync(user);
        var result = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        Assert.NotNull(result);
        Assert.Equal("testuser", result.Username);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetUserByEmailAsync_ShouldReturn_User_IF_EmailExists()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "hashedpassword",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        var result = await _userRepository.GetUserByEmailAsync("test@example.com");
        Assert.NotNull(result);
        Assert.Equal("testuser", result.Username);
    }
}