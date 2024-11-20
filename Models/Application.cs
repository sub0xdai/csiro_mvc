using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace csiro_mvc.Models
{
    public class Application
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Application name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
        [Display(Name = "Application Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 500 characters")]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; } = string.Empty;
        public User? User { get; set; }

        [Required]
        [Display(Name = "Course Type")]
        public CourseType CourseType { get; set; }

        [Required]
        [Range(3.0, 4.0, ErrorMessage = "GPA must be between 3.0 and 4.0")]
        public double GPA { get; set; }

        [Required]
        [Display(Name = "University")]
        public string University { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Cover Letter")]
        public string CoverLetter { get; set; } = string.Empty;

        [Display(Name = "CV File Path")]
        public string? CVFilePath { get; set; }

        [Display(Name = "Application Date")]
        public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Status is required")]
        [Display(Name = "Status")]
        public ApplicationStatus Status { get; set; } = ApplicationStatus.UnderReview;

        [Display(Name = "Created Date")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Display(Name = "Last Modified")]
        [DataType(DataType.DateTime)]
        public DateTime? LastModified { get; set; }

        public bool IsInvitedForInterview { get; set; } = false;
        public DateTime? InterviewInvitationDate { get; set; }

        // Navigation property
        public virtual ApplicationSettings? Settings { get; set; }
    }

    public enum CourseType
    {
        [Display(Name = "Master of Data Science")]
        DataScience,
        
        [Display(Name = "Master of Artificial Intelligence")]
        ArtificialIntelligence,
        
        [Display(Name = "Master of Cybersecurity")]
        Cybersecurity,
        
        [Display(Name = "Master of Computer Science")]
        ComputerScience
    }

    public enum ApplicationStatus
    {
        [Display(Name = "Pending")]
        Pending,
        
        [Display(Name = "Under Review")]
        UnderReview,
        
        [Display(Name = "Shortlisted")]
        Shortlisted,
        
        [Display(Name = "Accepted")]
        Accepted,
        
        [Display(Name = "Rejected")]
        Rejected
    }
}
