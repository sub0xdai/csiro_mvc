using Microsoft.AspNetCore.Identity;

namespace csiro_mvc.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Department { get; set; }
        public string? Position { get; set; }
        public string? Qualification { get; set; }
        public string? University { get; set; }
        public string? Role { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsProfileComplete { get; set; } = false;

        // Navigation properties
        public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

        public bool CheckProfileComplete()
        {
            return !string.IsNullOrWhiteSpace(FirstName) &&
                   !string.IsNullOrWhiteSpace(LastName) &&
                   !string.IsNullOrWhiteSpace(Department) &&
                   !string.IsNullOrWhiteSpace(Position) &&
                   !string.IsNullOrWhiteSpace(Qualification) &&
                   !string.IsNullOrWhiteSpace(University);
        }

        public void UpdateProfileCompletionStatus()
        {
            IsProfileComplete = CheckProfileComplete();
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
