using System.Security.Claims;
using csiro_mvc.Models;
using csiro_mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace csiro_mvc.Controllers
{
    [Authorize]
    public class ApplicationController : Controller
    {
        private readonly IApplicationService _applicationService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ApplicationController(
            IApplicationService applicationService,
            IWebHostEnvironment webHostEnvironment)
        {
            _applicationService = applicationService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Apply()
        {
            return View(new ApplicationForm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(ApplicationForm form, IFormFile cvFile)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }

            var application = new Application
            {
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Title = form.Title,
                Description = form.Description,
                CourseType = form.SelectedCourse,
                GPA = form.GPA,
                University = form.University,
                CoverLetter = form.CoverLetter,
                Status = ApplicationStatus.Submitted,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            if (cvFile != null && cvFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);
                
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + cvFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await cvFile.CopyToAsync(stream);
                }
                
                application.CVPath = uniqueFileName;
            }

            await _applicationService.CreateApplicationAsync(application);
            TempData["SuccessMessage"] = "Your application has been submitted successfully!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Index(string? searchTerm = null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Challenge();
            }

            var applications = string.IsNullOrEmpty(searchTerm)
                ? await _applicationService.GetApplicationsByUserIdAsync(userId)
                : await _applicationService.SearchApplicationsAsync(userId, searchTerm);

            return View(applications);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Application application, IFormFile? cvFile)
        {
            if (ModelState.IsValid)
            {
                application.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                application.Status = ApplicationStatus.Draft;
                application.CreatedAt = DateTime.UtcNow;
                application.UpdatedAt = DateTime.UtcNow;

                if (cvFile != null && cvFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    Directory.CreateDirectory(uploadsFolder); // Create if doesn't exist
                    
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + cvFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await cvFile.CopyToAsync(stream);
                    }
                    
                    application.CVPath = uniqueFileName;
                }

                await _applicationService.CreateApplicationAsync(application);
                return RedirectToAction(nameof(Index));
            }
            return View(application);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var application = await _applicationService.GetApplicationByIdAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (application.UserId != userId)
            {
                return Forbid();
            }

            return View(application);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Application application)
        {
            if (id != application.Id)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (application.UserId != userId)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                application.UpdatedAt = DateTime.UtcNow;
                await _applicationService.UpdateApplicationAsync(application);
                return RedirectToAction(nameof(Index));
            }
            return View(application);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var application = await _applicationService.GetApplicationByIdAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (application.UserId != userId)
            {
                return Forbid();
            }

            await _applicationService.DeleteApplicationAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var application = await _applicationService.GetApplicationByIdAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (application.UserId != userId)
            {
                return Forbid();
            }

            return View(application);
        }
    }
}
