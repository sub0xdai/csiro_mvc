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