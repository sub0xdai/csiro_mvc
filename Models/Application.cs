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
        [Display(Name = "Program Title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        public int ResearchProgramId { get; set; }
        public ResearchProgram? ResearchProgram { get; set; }

        [Required]
        [Display(Name = "Course")]
        public string CourseType { get; set; } = string.Empty;

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
        public string CoverLetter { get; set; } = string.Empty;

        [Required]
        [Display(Name = "CV File Path")]
        public string CVFilePath { get; set; } = string.Empty;

        public string CVPath { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }

        public virtual ApplicationSettings? Settings { get; set; }

        public virtual ICollection<ApplicationStatusHistory> StatusHistory { get; set; } = new List<ApplicationStatusHistory>();

        [Required]
        [Display(Name = "Status")]
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Draft;

        private DateTime _createdAt = DateTime.UtcNow;

        [Required]
        [Column(TypeName = "timestamp with time zone")]
        public DateTime CreatedAt
        {
            get => _createdAt;
            set => _createdAt = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        private DateTime? _updatedAt;

        [Column(TypeName = "timestamp with time zone")]
        public DateTime? UpdatedAt
        {
            get => _updatedAt;
            set => _updatedAt = value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : null;
        }

        [NotMapped]
        public DateTime LastModified => UpdatedAt ?? CreatedAt;

        /// <summary>
        /// The title of the research program this application is for
        /// </summary>
        [Required]
        public string ProgramTitle { get; set; } = string.Empty;
    }
}
