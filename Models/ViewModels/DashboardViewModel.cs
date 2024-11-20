using System;
using System.Collections.Generic;

namespace csiro_mvc.Models.ViewModels
{
    public class DashboardViewModel
    {
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Department { get; set; }
        public string? Role { get; set; }
        public DateTime LastLoginTime { get; set; }
        public int TotalApplications { get; set; }
        public List<ResearchProgram>? RecentPrograms { get; set; } = new();
    }
}
