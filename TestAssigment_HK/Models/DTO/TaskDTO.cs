using System.ComponentModel.DataAnnotations;
using TestAssigment_HK.Models.Enums;
using TaskStatus = TestAssigment_HK.Models.Enums.TaskStatus;

namespace TestAssigment_HK.Models.DTO;

public class TaskDTO
{
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    public DateTime? DueDate { get; set; }
    public TaskStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
}