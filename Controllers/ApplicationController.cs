using System.Security.Claims;
using csiro_mvc.Models;
using csiro_mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace csiro_mvc.Controllers
{
    [Authorize]
    public class ApplicationController : Controller
    {
        private readonly IApplicationService _applicationService;

        public ApplicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        public async Task<IActionResult> Index(string searchTerm)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var applications = string.IsNullOrEmpty(searchTerm)
                ? await _applicationService.GetApplicationsByUserIdAsync(userId)
                : await _applicationService.SearchApplicationsAsync(searchTerm);
            return View(applications);
        }

        public async Task<IActionResult> Details(int id)
        {
            var application = await _applicationService.GetApplicationByIdAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            // Ensure the user can only view their own applications
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (application.UserId != userId)
            {
                return Forbid();
            }

            return View(application);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Application application)
        {
            if (ModelState.IsValid)
            {
                application.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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

            var existingApplication = await _applicationService.GetApplicationByIdAsync(id);
            if (existingApplication == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (existingApplication.UserId != userId)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                application.UserId = userId;
                var updatedApplication = await _applicationService.UpdateApplicationAsync(id, application);
                if (updatedApplication == null)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(application);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
    }
}
