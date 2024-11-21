using System;
using System.Collections.Generic;
using csiro_mvc.Models;

namespace csiro_mvc.Models.ViewModels
{
    public class DashboardViewModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime LastLoginTime { get; set; }
        
        // Application Statistics
        public int TotalApplications { get; set; }
        public int PendingApplications { get; set; }
        public int ApprovedApplications { get; set; }
        public int RejectedApplications { get; set; }
        public double SuccessRate { get; set; }
        public TimeSpan AverageResponseTime { get; set; }

        // Collections
        public List<Application> Applications { get; set; } = new();
        public List<ResearchProgram> AvailablePrograms { get; set; } = new();
    }
}
