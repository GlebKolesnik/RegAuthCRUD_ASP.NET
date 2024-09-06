using TestAssigment_HK.Models.DTO;

namespace TestAssigment_HK.Services;

public interface IUserService
{
      Task<ResultDTO> RegisterUserAsync(RegisterUserDTO userDto);
      Task<AuthenticatedUserDTO> AuthenticateUserAsync(string email, string password);
      //Task<UserDTO> GetUserByIdAsync(Guid userId);
}