using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace csiro_mvc.Models
{
    public class ApplicationSettings
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Application")]
        public int ApplicationId { get; set; }

        [Display(Name = "Enable Notifications")]
        public bool NotificationsEnabled { get; set; } = true;

        [Required(ErrorMessage = "Theme selection is required")]
        [Display(Name = "Theme")]
        public string Theme { get; set; } = "Light";

        [Required(ErrorMessage = "Language selection is required")]
        [Display(Name = "Language")]
        public string Language { get; set; } = "English";

        [Required]
        [Range(0.0, 4.0)]
        [Display(Name = "Minimum GPA Requirement")]
        public double MinimumGPA { get; set; } = 3.0;

        // Navigation property
        public virtual Application? Application { get; set; }
    }
}
