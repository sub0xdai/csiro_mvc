using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using csiro_mvc.Models;
using csiro_mvc.Services;
using System.Security.Claims;

namespace csiro_mvc.Controllers
{
    [Authorize]
    public class ApplicationController : Controller
    {
        private readonly IApplicationService _applicationService;
        private readonly IApplicationSettingsService _settingsService;

        public ApplicationController(
            IApplicationService applicationService,
            IApplicationSettingsService settingsService)
        {
            _applicationService = applicationService;
            _settingsService = settingsService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var applications = await _applicationService.GetApplicationsByUserIdAsync(userId);
            return View(applications);
        }

        public IActionResult Create()
        {
            return View(new Application());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Application application)
        {
            if (!ModelState.IsValid)
                return View(application);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            application.UserId = userId;
            await _applicationService.CreateApplicationAsync(application);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var application = await _applicationService.GetApplicationByIdAsync(id);
            if (application == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null || application.UserId != userId)
            {
                return Unauthorized();
            }

            return View(application);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Application application)
        {
            if (id != application.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(application);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var existingApplication = await _applicationService.GetApplicationByIdAsync(id);
            if (existingApplication == null || existingApplication.UserId != userId)
            {
                return Unauthorized();
            }

            application.UserId = userId;
            await _applicationService.UpdateApplicationAsync(application);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Status()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var applications = await _applicationService.GetApplicationsByUserIdAsync(userId);
            return View(applications);
        }

        public async Task<IActionResult> Settings(int id)
        {
            var application = await _applicationService.GetApplicationByIdAsync(id);
            if (application == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null || application.UserId != userId)
            {
                return Unauthorized();
            }

            var settings = await _settingsService.GetSettingsByApplicationIdAsync(id);
            if (settings == null)
            {
                settings = new ApplicationSettings { ApplicationId = id };
                await _settingsService.CreateSettingsAsync(settings);
            }

            return View(settings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Settings(int id, ApplicationSettings settings)
        {
            if (id != settings.ApplicationId)
                return NotFound();

            if (!ModelState.IsValid)
                return View(settings);

            var application = await _applicationService.GetApplicationByIdAsync(id);
            if (application == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null || application.UserId != userId)
            {
                return Unauthorized();
            }

            await _settingsService.UpdateSettingsAsync(settings);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var application = await _applicationService.GetApplicationByIdAsync(id);
            if (application == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null || application.UserId != userId)
            {
                return Unauthorized();
            }

            await _applicationService.DeleteApplicationAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
