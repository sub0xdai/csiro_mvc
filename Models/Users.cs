// Models/User.cs
using Microsoft.AspNetCore.Identity;

namespace csiro_mvc.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Qualification { get; set; }
        public string University { get; set; }
        public double GPA { get; set; }
        public string CoverLetter { get; set; }
        public string CVPath { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

// Models/Application.cs
namespace csiro_mvc.Models
{
    public enum ApplicationStatus
    {
        Submitted,
        UnderReview,
        Shortlisted,
        Invited,
        Rejected
    }

    public class Application
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public ApplicationStatus Status { get; set; }
        public DateTime SubmissionDate { get; set; }
        public bool IsShortlisted { get; set; }
        public DateTime? InterviewDate { get; set; }
        public string AdminNotes { get; set; }
    }
}