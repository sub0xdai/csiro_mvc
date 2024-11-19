using System.ComponentModel.DataAnnotations;

namespace csiro_mvc.ViewModels
{
    public class AuthViewModel
    {
        // Login properties
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        // Register-specific properties
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

        // UI state
        public bool IsRegistering { get; set; }
    }
}
