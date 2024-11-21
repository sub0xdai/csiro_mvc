using System;
using csiro_mvc.Models;

namespace csiro_mvc.Models.ViewModels
{
    public class ApplicationStatusChange
    {
        public int ApplicationId { get; set; }
        public string ApplicationTitle { get; set; } = string.Empty;
        public ApplicationStatus OldStatus { get; set; }
        public ApplicationStatus NewStatus { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}
