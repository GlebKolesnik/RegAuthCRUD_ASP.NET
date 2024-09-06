using Castle.Core.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using TestAssigment_HK.Models;
using TestAssigment_HK.Models.DTO;
using TestAssigment_HK.Repositories;
using TestAssigment_HK.Services;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace TestAssigment_HK.Units;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userService = new UserService(_userRepositoryMock.Object, Mock.Of<IConfiguration>(),
            Mock.Of<ILogger<UserService>>());
    }

    [Fact]
    public async System.Threading.Tasks.Task RegisterUserAsync_Should_Return_Error_If_Email_Exists()
    {
        var userDto = new RegisterUserDTO { Email = "test@example.com", Password = "Test@1234" };
        _userRepositoryMock.Setup(r => r.GetUserByEmailAsync(userDto.Email)).ReturnsAsync(new User());
        var result = await _userService.RegisterUserAsync(userDto);
        Assert.False(result.Success);
        Assert.Equal("Email already in use", result.Message);
    }

    [Fact]
    public async System.Threading.Tasks.Task RegisterUserAsync_Should_Create_User_If_Email_Does_Not_Exist()
    {
        var userDto = new RegisterUserDTO { Email = "test@example.com", Password = "Test@1234" };
        _userRepositoryMock.Setup(r => r.GetUserByEmailAsync(userDto.Email)).ReturnsAsync((User)null);
        var result = await _userService.RegisterUserAsync(userDto);
        Assert.True(result.Success);
        _userRepositoryMock.Verify(r=> r.AddUserAsync(It.IsAny<User>()), Times.Once);
    }

}