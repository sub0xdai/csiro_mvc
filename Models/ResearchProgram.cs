using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace csiro_mvc.Models
{
    public class ResearchProgram
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(1, 10)]
        public int OpenPositions { get; set; }

        [Required]
        [StringLength(100)]
        public string Department { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Supervisor { get; set; } = string.Empty;

        [Required]
        [Range(0, 1000000)]
        public decimal FundingAmount { get; set; }

        private DateTime _startDate = DateTime.UtcNow;
        private DateTime _endDate = DateTime.UtcNow.AddYears(1);
        private DateTime _createdAt = DateTime.UtcNow;
        private DateTime? _updatedAt;

        [Required]
        [Column(TypeName = "timestamp with time zone")]
        public DateTime StartDate
        {
            get => _startDate;
            set => _startDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        [Required]
        [Column(TypeName = "timestamp with time zone")]
        public DateTime EndDate
        {
            get => _endDate;
            set => _endDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        [Column(TypeName = "timestamp with time zone")]
        public DateTime CreatedAt
        {
            get => _createdAt;
            set => _createdAt = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        [Column(TypeName = "timestamp with time zone")]
        public DateTime? UpdatedAt
        {
            get => _updatedAt;
            set => _updatedAt = value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : null;
        }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public List<Application> Applications { get; set; } = new();
    }
}
