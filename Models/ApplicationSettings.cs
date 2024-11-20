using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace csiro_mvc.Models
{
    public class ApplicationSettings
    {
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
        [RegularExpression("^[a-z]{2}$", ErrorMessage = "Language must be a valid 2-letter code")]
        public string Language { get; set; } = "en";

        [Required(ErrorMessage = "Time zone is required")]
        [Display(Name = "Time Zone")]
        public string TimeZone { get; set; } = "UTC";

        public double GPACutoff { get; set; } = 3.0;
        public int TopCandidatesCount { get; set; } = 10;
        public string EmailSenderName { get; set; } = "CSIRO HR";
        public string EmailTemplate { get; set; } = @"
Dear {FirstName},

Thank you for your application to CSIRO. We will review your application and get back to you soon.

Best regards,
{SenderName}
CSIRO Research Team
";

        // Navigation property
        public virtual Application? Application { get; set; }
    }
}
