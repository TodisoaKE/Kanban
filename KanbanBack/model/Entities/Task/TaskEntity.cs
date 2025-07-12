using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApprendreDotNet.model.Entities.user;

namespace ApprendreDotNet.model.Entities.kanban
{
    [Table("tasks", Schema = "public")]
    public class TaskEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Column("description")]
        public string? Description { get; set; }

        [Required]
        [Column("status")]
        public TaskStatus Status { get; set; } = TaskStatus.ToDo;

        [Column("assignedtouserid")]
        public int? AssignedToUserId { get; set; }

        [ForeignKey(nameof(AssignedToUserId))]
        public UserEntity? AssignedToUser { get; set; }

        [Column("createdat")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updatedat")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<TaskHistoryEntity>? History { get; set; }
    }

    public enum TaskStatus
    {
        ToDo = 1,
        InProgress = 2,
        Done = 3
    }
}
