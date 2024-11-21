using System.ComponentModel.DataAnnotations;

namespace csiro_mvc.Models
{
    public enum ApplicationStatus
    {
        [Display(Name = "Draft")]
        Draft,
        
        [Display(Name = "Submitted")]
        Submitted,
        
        [Display(Name = "Under Review")]
        UnderReview,
        
        [Display(Name = "Approved")]
        Approved,
        
        [Display(Name = "Accepted")]
        Accepted,
        
        [Display(Name = "Rejected")]
        Rejected,
        
        [Display(Name = "Withdrawn")]
        Withdrawn,
        
        [Display(Name = "Pending")]
        Pending
    }
}
