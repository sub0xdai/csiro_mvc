using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace csiro_mvc.Models
{
    public class ApplicationStatusHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ApplicationId { get; set; }

        [Required]
        public ApplicationStatus OldStatus { get; set; }

        [Required]
        public ApplicationStatus NewStatus { get; set; }

        [Required]
        public DateTime ChangedAt { get; set; }

        [Required]
        public string ChangedBy { get; set; } = string.Empty;

        // Navigation property
        [ForeignKey("ApplicationId")]
        public virtual Application? Application { get; set; }
    }
}
