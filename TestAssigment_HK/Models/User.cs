namespace TestAssigment_HK.Models;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    //Хранилище задач пользователя, того который авторизован
    public ICollection<Task> Tasks { get; set; } // Вопрос по ICollection

}