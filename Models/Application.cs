using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace csiro_mvc.Models
{
    public class Application
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

        [Required]
        [Display(Name = "Course Type")]
        public CourseType CourseType { get; set; }

        [Required]
        [Range(3.0, 4.0, ErrorMessage = "GPA must be between 3.0 and 4.0")]
        public double GPA { get; set; }

        [Required]
        [Display(Name = "University")]
        public string University { get; set; }

        [Required]
        [Display(Name = "Cover Letter")]
        public string CoverLetter { get; set; }

        [Display(Name = "CV File Path")]
        public string? CVFilePath { get; set; }

        [Display(Name = "Application Date")]
        public DateTime ApplicationDate { get; set; }

        [Display(Name = "Application Status")]
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;

        public bool IsInvitedForInterview { get; set; } = false;
        public DateTime? InterviewInvitationDate { get; set; }
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
