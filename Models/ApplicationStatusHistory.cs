using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace csiro_mvc.Models
{
    public class ApplicationStatusHistory
    {
        [Key]
        public int Id { get; set; }
        
        public int ApplicationId { get; set; }
        [ForeignKey("ApplicationId")]
        public Application? Application { get; set; }
        
        public ApplicationStatus OldStatus { get; set; }
        public ApplicationStatus Status { get; set; }  // Current status
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
        public string? Comment { get; set; }
        
        public ApplicationStatusHistory()
        {
            ChangedAt = DateTime.UtcNow;
        }
    }
}
