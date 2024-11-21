using System.Collections.Generic;
using csiro_mvc.Models;

namespace csiro_mvc.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public IEnumerable<Application> Applications { get; set; } = new List<Application>();
        public string CurrentSort { get; set; } = "";
        public string SearchString { get; set; } = "";
        public double? GPAFilter { get; set; }
        public double MinGPARequirement { get; set; }
    }
}
