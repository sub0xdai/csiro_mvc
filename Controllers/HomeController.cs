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

            var roles = await _userManager.GetRolesAsync(user);
            var viewModel = new DashboardViewModel
            {
                UserName = user.UserName ?? "User",
                FirstName = user.FirstName ?? string.Empty,
                LastName = user.LastName ?? string.Empty,
                Department = user.Department ?? "Not Set",
                Role = roles.FirstOrDefault() ?? "User",
                LastLoginTime = DateTime.Now,
                TotalApplications = await _applicationService.GetUserApplicationsCountAsync(user.Id),
                RecentPrograms = await _applicationService.GetRecentProgramsAsync()
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
