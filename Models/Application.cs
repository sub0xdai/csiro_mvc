using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace csiro_mvc.Models
{
    public enum Course
    {
        [Display(Name = "Master of Data Science")]
        DataScience,
        [Display(Name = "Master of Artificial Intelligence")]
        ArtificialIntelligence,
        [Display(Name = "Master of Information Technology")]
        InformationTechnology,
        [Display(Name = "Master of Science (Statistics)")]
        Statistics
    }

    public class Application
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Course")]
        public Course CourseType { get; set; }

        [Required]
        [Range(3.0, 4.0, ErrorMessage = "GPA must be between 3.0 and 4.0")]
        public double GPA { get; set; }

        [Required]
        [Display(Name = "University")]
        [StringLength(100)]
        public string University { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Cover Letter")]
        [DataType(DataType.MultilineText)]
        [StringLength(2000)]
        public string CoverLetter { get; set; } = string.Empty;

        [Display(Name = "CV")]
        public string? CVPath { get; set; }

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
