using System.Security.Claims;
using csiro_mvc.Models;
using csiro_mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Serilog;
using ILogger = Serilog.ILogger;

namespace csiro_mvc.Controllers
{
    [Authorize]
    public class ApplicationController : Controller
    {
        private readonly IApplicationService _applicationService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger _logger;

        public ApplicationController(
            IApplicationService applicationService,
            IWebHostEnvironment webHostEnvironment)
        {
            _applicationService = applicationService;
            _webHostEnvironment = webHostEnvironment;
            _logger = Log.ForContext<ApplicationController>();
        }

        public IActionResult Apply()
        {
            return View(new ApplicationForm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(ApplicationForm form)
        {
            try
            {
                _logger.Information("Starting application submission process");

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    _logger.Warning("Model state is invalid: {Errors}", string.Join("; ", errors));
                    
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                    return View(form);
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.Error("User ID is null or empty");
                    ModelState.AddModelError("", "User not authenticated. Please log in again.");
                    return View(form);
                }

                _logger.Information("Creating application for user: {UserId}", userId);

                var application = new Application
                {
                    UserId = userId,
                    Title = form.Title,
                    CourseType = form.SelectedCourse,
                    GPA = form.GPA,
                    University = form.University,
                    CoverLetter = form.CoverLetter,
                    Status = ApplicationStatus.Pending,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                if (form.CVFile != null && form.CVFile.Length > 0)
                {
                    _logger.Information("Processing CV file: {FileName}", form.CVFile.FileName);
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    Directory.CreateDirectory(uploadsFolder);
                    
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + form.CVFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await form.CVFile.CopyToAsync(stream);
                    }
                    
                    application.CVPath = uniqueFileName;
                    _logger.Information("CV file saved successfully: {FilePath}", filePath);
                }

                _logger.Information("Calling CreateApplicationAsync");
                await _applicationService.CreateApplicationAsync(application);
                _logger.Information("Application created successfully with ID: {ApplicationId}", application.Id);
                
                TempData["SuccessMessage"] = "Your application has been submitted successfully!";
                _logger.Information("Redirecting to Success page");
                return RedirectToAction(nameof(Success), new { id = application.Id });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while processing application submission");
                ModelState.AddModelError("", $"An error occurred while saving your application: {ex.Message}");
                return View(form);
            }
        }

        public async Task<IActionResult> Success(int id)
        {
            _logger.Information("Loading success page for application ID: {ApplicationId}", id);
            var application = await _applicationService.GetApplicationByIdAsync(id);
            
            if (application == null)
            {
                _logger.Warning("Application not found: {ApplicationId}", id);
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (application.UserId != userId)
            {
                _logger.Warning("Unauthorized access attempt to application: {ApplicationId} by user: {UserId}", id, userId);
                return Forbid();
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

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _logger.Information("Loading applications for user: {UserId}", userId);
            var applications = await _applicationService.GetApplicationsAsync(userId);
            return View(applications);
        }
    }
}
