using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Win32.SafeHandles;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using TestAssigment_HK.Models;
using TestAssigment_HK.Models.DTO;
using TestAssigment_HK.Repositories;

namespace TestAssigment_HK.Services;

public class UserService : IUserService
{
    public readonly IUserRepository _userRepository;
    public readonly IConfiguration _configuration;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, IConfiguration configuration, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<ResultDTO> RegisterUserAsync(RegisterUserDTO userDto)
    {
        _logger.LogInformation("Starting registration for user {Email}", userDto.Email);
        var passwordValidationResult = ValidatePasswordComplexity(userDto.Password);
        if (!passwordValidationResult.Success)
        {
            return new ResultDTO { Success = false, Message = passwordValidationResult.Message };
        }
        var existingUser = await _userRepository.GetUserByEmailAsync(userDto.Email);
        if (existingUser!=null)
        {
            _logger.LogWarning("Registration failed. Email {Email} already in use", userDto.Email);
            return new ResultDTO {Success = false, Message = "Email already in use"};
        }

        var hashPassword = HashPassword(userDto.Password);
        var newUser = new User
        {
            Id = Guid.NewGuid(),
            Username = userDto.Username,
            Email = userDto.Email,
            PasswordHash = hashPassword,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await _userRepository.AddUserAsync(newUser);
        _logger.LogInformation("User {Email} successfully registered", userDto.Email);
        return new ResultDTO {Success = true, Message = "User successfully registered"};
    }

    public async Task<AuthenticatedUserDTO> AuthenticateUserAsync(string emailOrUsername, string password)
    {
        var user = await _userRepository.GetUserByEmailOrUsernameAsync(emailOrUsername);
        if (user == null || !VerifyPassword(password, user.PasswordHash))
        {
            return null;
        }

        var token = GenerateJwtToken(user);
        return new AuthenticatedUserDTO
        {
            Username = user.Username,
            Email = user.Email,
            Token = token
        };
    }

    private bool VerifyPassword(string password, string storedHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, storedHash);
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _configuration["Jwt:Issuer"],  
            Audience = _configuration["Jwt:Audience"] 
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private ResultDTO ValidatePasswordComplexity(string password)
    {
        if (password.Length < 8 )
        {
            return new ResultDTO { Success = false, Message = "Password must be at least 8 characters long" };
        }

        if (!password.Any(char.IsLower))
        {
            return new ResultDTO { Success = false, Message = "Password must contain at least one lowercase letter" };
        }

        if (!password.Any(char.IsUpper))
        {
            return new ResultDTO { Success = false, Message = "Password must contain at least one uppercase letter" };
        }

        if (!password.Any(char.IsDigit))
        {
            return new ResultDTO { Success = false, Message = "Password must contain at least one number" };
        }

        if (!password.Any(ch => "!@#$%^&*()_+-=[]{}|;':\",.<>?/".Contains(ch)))
        {
            return new ResultDTO { Success = false, Message = "Password must contain at least one special character" };
        }

        return new ResultDTO { Success = true, Message = "Password is valid" };
    }
}