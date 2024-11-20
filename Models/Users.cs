// Models/User.cs
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace csiro_mvc.Models
{
    public class User : IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Qualification")]
        public string Qualification { get; set; } = string.Empty;

        [Required]
        [Display(Name = "University")]
        public string University { get; set; } = string.Empty;

        [Required]
        [Range(0, 4.0, ErrorMessage = "GPA must be between 0 and 4.0")]
        public double GPA { get; set; }

        [Required]
        [Display(Name = "Cover Letter")]
        public string CoverLetter { get; set; } = string.Empty;

        [Required]
        [Display(Name = "CV")]
        public string CVPath { get; set; } = string.Empty;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}