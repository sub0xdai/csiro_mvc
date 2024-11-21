using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace csiro_mvc.Models
{
    public class ApplicationForm
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please select a research program")]
        [Display(Name = "Research Program")]
        public string ProgramTitle { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select your course")]
        [Display(Name = "Course")]
        public string CourseType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select your university")]
        [Display(Name = "University")]
        public string University { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your GPA")]
        [Range(3.0, 4.0, ErrorMessage = "GPA must be between 3.0 and 4.0")]
        [Display(Name = "GPA")]
        public double GPA { get; set; }

        [Required(ErrorMessage = "Please provide a cover letter")]
        [Display(Name = "Cover Letter")]
        [StringLength(2000, MinimumLength = 100, ErrorMessage = "Cover letter must be between 100 and 2000 characters")]
        [DataType(DataType.MultilineText)]
        public string CoverLetter { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please upload your CV")]
        [Display(Name = "CV")]
        public IFormFile CVFile { get; set; }

        public List<SelectListItem> AvailablePrograms { get; set; }
        public List<SelectListItem> Universities { get; set; }

        public ApplicationForm()
        {
            AvailablePrograms = new List<SelectListItem>();
            Universities = new List<SelectListItem>();
        }

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "Available Positions")]
        public int NumberOfPositions { get; set; }
    }
}
