using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using csiro_mvc.Models;
using csiro_mvc.Models.ViewModels;
using csiro_mvc.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Serilog;
using System.Diagnostics;

namespace csiro_mvc.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ApplicationController : Controller
    {
        private readonly IApplicationService _applicationService;
        private readonly IResearchProgramService _researchProgramService;
        private readonly IUniversityService _universityService;
        private readonly ILogger<ApplicationController> _logger;

        public ApplicationController(
            IApplicationService applicationService,
            IResearchProgramService researchProgramService,
            IUniversityService universityService,
            ILogger<ApplicationController> logger)
        {
            _applicationService = applicationService;
            _researchProgramService = researchProgramService;
            _universityService = universityService;
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                var applications = await _applicationService.GetApplicationsAsync(userId);
                return View(applications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving applications");
                return View("Error");
            }
        }

        [HttpGet]
        [Route("Apply")]
        public async Task<IActionResult> Apply()
        {
            try
            {
                var programs = await _researchProgramService.GetAllProgramsAsync();
                var universities = _universityService.GetTop100Universities();
                
                var form = new ApplicationForm
                {
                    AvailablePrograms = programs.Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Title
                    }).ToList(),
                    Universities = universities.Select(u => new SelectListItem
                    {
                        Value = u.Name,
                        Text = $"{u.Name} ({u.Country})"
                    }).ToList()
                };
                return View(form);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading application form");
                return View("Error");
            }
        }

        [HttpPost]
        [Route("Submit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(ApplicationForm form)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid. Errors: {Errors}", 
                    string.Join(", ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)));
                
                var programs = await _researchProgramService.GetAllProgramsAsync();
                var universities = _universityService.GetTop100Universities();
                form.AvailablePrograms = programs.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Title
                }).ToList();
                form.Universities = universities.Select(u => new SelectListItem
                {
                    Value = u.Name,
                    Text = $"{u.Name} ({u.Country})"
                }).ToList();
                return View("Apply", form);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                if (!int.TryParse(form.ProgramTitle, out int programId))
                {
                    ModelState.AddModelError("ProgramTitle", "Invalid program selection.");
                    var programs = await _researchProgramService.GetAllProgramsAsync();
                    var universities = _universityService.GetTop100Universities();
                    form.AvailablePrograms = programs.Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Title
                    }).ToList();
                    form.Universities = universities.Select(u => new SelectListItem
                    {
                        Value = u.Name,
                        Text = $"{u.Name} ({u.Country})"
                    }).ToList();
                    return View("Apply", form);
                }

                var application = await _applicationService.CreateApplicationAsync(userId, programId, form);
                TempData["SuccessMessage"] = "Your application has been submitted successfully! You can track its status on your dashboard.";
                return RedirectToAction(nameof(Success), new { id = application.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting application");
                ModelState.AddModelError("", "An error occurred while submitting your application. Please try again.");
                var programs = await _researchProgramService.GetAllProgramsAsync();
                var universities = _universityService.GetTop100Universities();
                form.AvailablePrograms = programs.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Title
                }).ToList();
                form.Universities = universities.Select(u => new SelectListItem
                {
                    Value = u.Name,
                    Text = $"{u.Name} ({u.Country})"
                }).ToList();
                return View("Apply", form);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                var application = await _applicationService.GetApplicationByIdAsync(id);
                
                if (application == null || application.UserId != userId)
                {
                    return NotFound();
                }

                return View(application);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving application details");
                return View("Error");
            }
        }

        [HttpGet("{id}/edit")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var application = await _applicationService.GetApplicationByIdAsync(id);
                if (application == null)
                    return NotFound();

                var programs = await _researchProgramService.GetAllProgramsAsync();
                var universities = _universityService.GetTop100Universities();
                var form = new ApplicationForm
                {
                    AvailablePrograms = programs.Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Title
                    }).ToList(),
                    Universities = universities.Select(u => new SelectListItem
                    {
                        Value = u.Name,
                        Text = $"{u.Name} ({u.Country})"
                    }).ToList()
                };
                return View(form);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving application for editing");
                return View("Error");
            }
        }

        [HttpPost("{id}/edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Application application)
        {
            if (id != application.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(application);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                await _applicationService.UpdateApplicationAsync(id, application);
                TempData["SuccessMessage"] = "Application updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating application");
                ModelState.AddModelError("", "An error occurred while updating your application. Please try again.");
                return View(application);
            }
        }

        [HttpGet("{id}/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                var application = await _applicationService.GetApplicationByIdAsync(id);
                
                if (application == null || application.UserId != userId)
                {
                    return NotFound();
                }

                return View(application);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving application for deletion");
                return View("Error");
            }
        }

        [HttpPost("{id}/delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                await _applicationService.DeleteApplicationAsync(id);
                TempData["SuccessMessage"] = "Application deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting application");
                return View("Error");
            }
        }

        [HttpGet("activity")]
        public async Task<IActionResult> GetActivity()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            try
            {
                var activities = await _applicationService.GetStatusChangesAsync(userId);
                return Ok(activities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving application activity");
                return StatusCode(500, "An error occurred while retrieving activity");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [Route("Success/{id}")]
        public async Task<IActionResult> Success(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                var application = await _applicationService.GetApplicationByIdAsync(id);
                
                if (application == null || application.UserId != userId)
                {
                    return NotFound();
                }

                return View(application);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving application for success page");
                return View("Error");
            }
        }
    }
}
