using System.Runtime.InteropServices.JavaScript;
using TestAssigment_HK.Models.Enums;
using TaskStatus = TestAssigment_HK.Models.Enums.TaskStatus;

namespace TestAssigment_HK.Models;

public class Task
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? DueDate { get; set; }
    public TaskStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }

}