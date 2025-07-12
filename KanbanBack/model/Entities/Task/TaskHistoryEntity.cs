using ApprendreDotNet.model.Entities.user;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApprendreDotNet.model.Entities.kanban
{
    [Table("taskhistories", Schema = "public")]
    public class TaskHistoryEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("taskid")]
        public int TaskId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(TaskId))]
        public TaskEntity? Task { get; set; }

        [Required]
        [Column("changedbyuserid")]
        public int ChangedByUserId { get; set; }

        [ForeignKey(nameof(ChangedByUserId))]
        public UserEntity? ChangedByUser { get; set; }

        [Required]
        [Column("changetype")]
        public string ChangeType { get; set; }

        [Column("oldvalue")]
        public string? OldValue { get; set; }

        [Column("newvalue")]
        public string? NewValue { get; set; }

        [Required]
        [Column("changedate")]
        public DateTime ChangeDate { get; set; } = DateTime.UtcNow;

        /*public enum ChangeTypeEnum
        {
            Creation = 1,
            StatusChange = 2,
            AssignmentChange = 3,
        }*/
    }
}
