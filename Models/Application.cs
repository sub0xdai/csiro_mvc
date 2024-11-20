using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace csiro_mvc.Models
{
    public class Application
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string CourseType { get; set; } = null!;

        public ApplicationStatus Status { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [NotMapped]
        public DateTime LastModified => UpdatedAt ?? CreatedAt;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }
        public virtual ApplicationSettings? Settings { get; set; }
    }

    public enum ApplicationStatus
    {
        Draft,
        Submitted,
        UnderReview,
        InReview,
        Pending,
        Approved,
        Rejected,
        Withdrawn
    }
}
