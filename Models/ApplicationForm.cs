using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace csiro_mvc.Models
{
    public class ApplicationForm
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Course")]
        public Course SelectedCourse { get; set; }

        [Required]
        [Range(3.0, 4.0, ErrorMessage = "GPA must be between 3.0 and 4.0")]
        public double GPA { get; set; }

        [Required]
        [Display(Name = "University")]
        [StringLength(100)]
        public string University { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Cover Letter")]
        [StringLength(2000)]
        [DataType(DataType.MultilineText)]
        public string CoverLetter { get; set; } = string.Empty;

        [Display(Name = "CV")]
        public IFormFile? CVFile { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
