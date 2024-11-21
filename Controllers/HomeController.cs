using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using csiro_mvc.Models;
using csiro_mvc.Models.ViewModels;
using csiro_mvc.Services;
using System.Threading.Tasks;
using System.Linq;

namespace csiro_mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IApplicationService _applicationService;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(
            ILogger<HomeController> logger, 
            IApplicationService applicationService,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _applicationService = applicationService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction(nameof(Dashboard));
            }
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Dashboard()
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound();
                }

                var applications = await _applicationService.GetApplicationsByUserIdAsync(user.Id);
                var recentPrograms = await _applicationService.GetRecentProgramsAsync(5); // Get 5 most recent programs

                // Calculate application statistics
                var totalApps = applications.Count();
                var pendingApps = applications.Count(a => a.Status == ApplicationStatus.Pending);
                var approvedApps = applications.Count(a => a.Status == ApplicationStatus.Approved);
                var rejectedApps = applications.Count(a => a.Status == ApplicationStatus.Rejected);
                
                // Calculate success rate (approved applications / total completed applications)
                var completedApps = approvedApps + rejectedApps;
                var successRate = completedApps > 0 ? (double)approvedApps / completedApps * 100 : 0;

                // Calculate average response time for completed applications
                var averageResponseTime = TimeSpan.Zero;
                var completedApplications = applications.Where(a => 
                    a.Status == ApplicationStatus.Approved || a.Status == ApplicationStatus.Rejected);
                
                if (completedApplications.Any())
                {
                    var totalResponseTime = completedApplications.Sum(a => 
                        ((a.UpdatedAt ?? DateTime.Now) - a.CreatedAt).TotalMilliseconds);
                    averageResponseTime = TimeSpan.FromMilliseconds(totalResponseTime / completedApplications.Count());
                }

                var viewModel = new DashboardViewModel
                {
                    FirstName = user.FirstName,
                    Department = user.Department ?? "",
                    Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "User",
                    LastLoginTime = DateTime.Now,
                    Applications = applications.Take(5),
                    TotalApplications = totalApps,
                    PendingApplications = pendingApps,
                    ApprovedApplications = approvedApps,
                    RejectedApplications = rejectedApps,
                    SuccessRate = successRate,
                    AverageResponseTime = averageResponseTime,
                    RecentPrograms = recentPrograms,
                    RecentStatusChanges = await _applicationService.GetRecentStatusChangesAsync(User.FindFirstValue(ClaimTypes.NameIdentifier))
                };

                return View(viewModel);
            }

        public IActionResult Programs()
        {
            var programs = new List<ResearchProgram>
            {
                new ResearchProgram { Id = 1, Name = "Data61", Description = "Digital and data innovation", OpenPositions = 5 },
                new ResearchProgram { Id = 2, Name = "Agriculture and Food", Description = "Sustainable agricultural solutions", OpenPositions = 3 },
                new ResearchProgram { Id = 3, Name = "Manufacturing", Description = "Advanced manufacturing technologies", OpenPositions = 4 },
                new ResearchProgram { Id = 4, Name = "Health and Biosecurity", Description = "Health and biosecurity research", OpenPositions = 2 },
                new ResearchProgram { Id = 5, Name = "Energy", Description = "Sustainable energy solutions", OpenPositions = 6 }
            };
            return View(programs);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
