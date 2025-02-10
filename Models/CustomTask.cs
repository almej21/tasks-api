using System.ComponentModel.DataAnnotations;

namespace tasks_api.Classes
{
    public class CustomTask
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Title { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public DateTime? DueDate { get; set; }

        [Required]
        public CustomTaskStatus Status { get; set; }
    }
    public enum CustomTaskStatus
    {
        Pending,
        InProgress,
        Completed,
        Overdue
    }
}
