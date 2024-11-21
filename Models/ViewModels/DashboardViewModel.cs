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

        // Recent Activity
        public IEnumerable<Application> Applications { get; set; } = new List<Application>();
        public IEnumerable<ResearchProgram> RecentPrograms { get; set; } = new List<ResearchProgram>();
        public IEnumerable<ApplicationStatusChange> RecentStatusChanges { get; set; } = new List<ApplicationStatusChange>();
    }
}
