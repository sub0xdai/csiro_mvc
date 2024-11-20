using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using csiro_mvc.Models;
using csiro_mvc.Services;
using System.Threading.Tasks;
using System.Linq;

namespace csiro_mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IApplicationService _applicationService;

        public HomeController(ILogger<HomeController> logger, IApplicationService applicationService)
        {
            _logger = logger;
            _applicationService = applicationService;
        }

        public IActionResult Index()
        {
            return View();
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
