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
        private readonly IResearchProgramService _researchProgramService;

        public HomeController(
            ILogger<HomeController> logger, 
            IApplicationService applicationService,
            UserManager<ApplicationUser> userManager,
            IResearchProgramService researchProgramService)
        {
            _logger = logger;
            _applicationService = applicationService;
            _userManager = userManager;
            _researchProgramService = researchProgramService;
        }

        public IActionResult Index()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction(nameof(Dashboard));
            }
            return View();
        }

        public IActionResult Details(int id)
    {
            var programs = new List<ResearchProgram>
            {
                new ResearchProgram { 
                    Id = 1, 
                    Title = "Data61", 
                    Description = "Data61 is CSIRO's data and digital specialist arm, and Australia's leading digital research network. We partner with industry, government and universities to conduct cutting-edge research in artificial intelligence, robotics, cybersecurity, and data analytics. Our program focuses on solving complex problems through innovative digital solutions, machine learning, and advanced data science techniques. Join us in shaping Australia's digital future through groundbreaking research and development in areas such as privacy-preserving technologies, autonomous systems, and blockchain applications.", 
                    OpenPositions = 5 
                },
                new ResearchProgram { 
                    Id = 2, 
                    Title = "Agriculture and Food", 
                    Description = "The Agriculture and Food research program is dedicated to transforming the Australian agriculture sector through innovative research and sustainable practices. We work on critical challenges including food security, climate-resilient farming, and sustainable agricultural systems. Our research spans across soil health, crop improvement, livestock management, and precision agriculture technologies. We're developing next-generation farming solutions using advanced genomics, digital agriculture, and sustainable farming practices to ensure Australia's agricultural future while addressing global food security challenges.", 
                    OpenPositions = 3 
                },
                new ResearchProgram { 
                    Id = 3, 
                    Title = "Manufacturing", 
                    Description = "Our Manufacturing research program is at the forefront of industrial innovation, focusing on advanced manufacturing technologies and Industry 4.0 solutions. We're revolutionizing traditional manufacturing through research in 3D printing, smart materials, and automated production systems. Our work encompasses developing new materials, improving manufacturing processes, and creating innovative solutions for sustainable production. Join us in creating the factories of the future, where we combine artificial intelligence, robotics, and advanced materials to drive manufacturing efficiency and sustainability.", 
                    OpenPositions = 4 
                },
                new ResearchProgram { 
                    Id = 4, 
                    Title = "Health and Biosecurity", 
                    Description = "The Health and Biosecurity program conducts vital research to protect Australia's health and biosecurity. We focus on emerging infectious diseases, vaccine development, and biosecurity measures to safeguard our agriculture and communities. Our research encompasses pandemic preparedness, antimicrobial resistance, and environmental health challenges. We utilize advanced molecular biology, genomics, and data science to develop innovative solutions for disease prevention and control. Our work directly contributes to national health security and the development of new therapeutic approaches.", 
                    OpenPositions = 2 
                },
                new ResearchProgram { 
                    Id = 5, 
                    Title = "Energy", 
                    Description = "CSIRO's Energy research program is leading Australia's transition to a sustainable energy future. We conduct pioneering research in renewable energy technologies, energy storage solutions, and smart grid systems. Our work spans solar thermal technologies, hydrogen fuel systems, battery storage, and grid integration of renewable energy. We're developing innovative solutions for clean energy generation, improving energy efficiency, and reducing carbon emissions. Join us in creating breakthrough technologies that will power Australia's sustainable future while addressing global climate challenges.", 
                    OpenPositions = 6 
                }
        };

            var program = programs.FirstOrDefault(p => p.Id == id);
            if (program == null)
            {
                return NotFound();
            }

            return View(program);
        }

        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var applications = await _applicationService.GetApplicationsAsync(user.Id);
                var programs = await _researchProgramService.GetAllProgramsAsync();

                var viewModel = new DashboardViewModel
                {
                    FirstName = user.FirstName ?? "User",
                    Department = user.Department ?? "Not Set",
                    Role = user.Role ?? "Applicant",
                    LastLoginTime = user.LastLoginTime ?? DateTime.UtcNow,
                    Applications = applications,
                    AvailablePrograms = programs
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Dashboard action");
                return RedirectToAction(nameof(Error));
            }
        }

        public IActionResult Programs()
        {
            var programs = new List<ResearchProgram>
            {
                new ResearchProgram { 
                    Id = 1, 
                    Title = "Data61", 
                    Description = "Data61 is CSIRO's data and digital specialist arm, and Australia's leading digital research network. We partner with industry, government and universities to conduct cutting-edge research in artificial intelligence, robotics, cybersecurity, and data analytics.", 
                    OpenPositions = 5 
                },
                new ResearchProgram { 
                    Id = 2, 
                    Title = "Agriculture and Food", 
                    Description = "The Agriculture and Food research program is dedicated to transforming the Australian agriculture sector through innovative research and sustainable practices. We work on critical challenges including food security, climate-resilient farming, and sustainable agricultural systems.", 
                    OpenPositions = 3 
                },
                new ResearchProgram { 
                    Id = 3, 
                    Title = "Manufacturing", 
                    Description = "Our Manufacturing research program is at the forefront of industrial innovation, focusing on advanced manufacturing technologies and Industry 4.0 solutions. We're revolutionizing traditional manufacturing through research in 3D printing, smart materials, and automated production systems.", 
                    OpenPositions = 4 
                },
                new ResearchProgram { 
                    Id = 4, 
                    Title = "Health and Biosecurity", 
                    Description = "The Health and Biosecurity program conducts vital research to protect Australia's health and biosecurity. We focus on emerging infectious diseases, vaccine development, and biosecurity measures to safeguard our agriculture and communities.", 
                    OpenPositions = 2 
                },
                new ResearchProgram { 
                    Id = 5, 
                    Title = "Energy", 
                    Description = "CSIRO's Energy research program is leading Australia's transition to a sustainable energy future. We conduct pioneering research in renewable energy technologies, energy storage solutions, and smart grid systems.", 
                    OpenPositions = 6 
                }
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
