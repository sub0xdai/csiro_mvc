using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using csiro_mvc.Models;
using csiro_mvc.Models.ViewModels;
using csiro_mvc.Services;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace csiro_mvc.Controllers
{
    [Authorize(Policy = "RequireAdminRole")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            IAdminService adminService,
            ILogger<AdminController> logger)
        {
            _adminService = adminService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string sortOrder = "", string searchString = "", double? gpaFilter = null)
        {
            try
            {
                var applications = await _adminService.GetAllApplicationsAsync();
                var minGPA = await _adminService.GetMinimumGPARequirementAsync();

                // Apply GPA filter
                if (gpaFilter.HasValue)
                {
                    applications = applications.Where(a => a.GPA >= gpaFilter.Value).ToList();
                }

                // Apply search
                if (!string.IsNullOrEmpty(searchString))
                {
                    applications = applications.Where(a => 
                        a.ResearchProgram?.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase) == true ||
                        a.User?.University?.Contains(searchString, StringComparison.OrdinalIgnoreCase) == true
                    ).ToList();
                }

                // Apply sorting
                applications = sortOrder switch
                {
                    "gpa_desc" => applications.OrderByDescending(a => a.GPA).ToList(),
                    "gpa" => applications.OrderBy(a => a.GPA).ToList(),
                    "date_desc" => applications.OrderByDescending(a => a.CreatedAt).ToList(),
                    "date" => applications.OrderBy(a => a.CreatedAt).ToList(),
                    _ => applications.OrderByDescending(a => a.CreatedAt).ToList()
                };

                var viewModel = new AdminDashboardViewModel
                {
                    Applications = applications,
                    CurrentSort = sortOrder,
                    SearchString = searchString,
                    GPAFilter = gpaFilter,
                    MinGPARequirement = minGPA
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving admin dashboard data");
                TempData["ErrorMessage"] = "An error occurred while loading the dashboard.";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateGPACutoff(double minimumGPA)
        {
            try
            {
                await _adminService.UpdateMinimumGPARequirementAsync(minimumGPA);
                TempData["SuccessMessage"] = "GPA cutoff updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating GPA cutoff");
                TempData["ErrorMessage"] = "Failed to update GPA cutoff.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendInvitation(int applicationId)
        {
            try
            {
                await _adminService.SendInterviewInvitationAsync(applicationId);
                TempData["SuccessMessage"] = "Interview invitation sent successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending interview invitation");
                TempData["ErrorMessage"] = "Failed to send interview invitation.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
