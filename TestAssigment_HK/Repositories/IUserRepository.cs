using TestAssigment_HK.Controllers;
using TestAssigment_HK.Models;
using Task = System.Threading.Tasks.Task;

namespace TestAssigment_HK.Repositories;

public interface IUserRepository
{
    Task<User> GetUserByEmailAsync(string email);
    Task<User> GetUserByEmailOrUsernameAsync(string emailOrUserName);
    Task<User> GetUserByIdAsync(Guid userId);
    Task AddUserAsync(User user);
}