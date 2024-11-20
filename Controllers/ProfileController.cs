using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using csiro_mvc.Models;
using csiro_mvc.Models.ViewModels;

namespace csiro_mvc.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(
            UserManager<ApplicationUser> userManager,
            ILogger<ProfileController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new ProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                Department = user.Department ?? "",
                Position = user.Position ?? "",
                Qualification = user.Qualification ?? "",
                University = user.University ?? ""
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Complete()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new ProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                Department = user.Department ?? "",
                Position = user.Position ?? "",
                Qualification = user.Qualification ?? "",
                University = user.University ?? ""
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete([FromForm] ProfileViewModel model)
        {
            _logger.LogInformation("Profile completion attempt received for model: {@Model}", model);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid: {@ModelState}", ModelState);
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogWarning("User not found during profile completion");
                return NotFound();
            }

            try
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Department = model.Department;
                user.Position = model.Position;
                user.Qualification = model.Qualification;
                user.University = model.University;
                user.UpdatedAt = DateTime.UtcNow;
                user.UpdateProfileCompletionStatus();

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User {UserId} completed their profile successfully", user.Id);
                    TempData["StatusMessage"] = "Your profile has been updated successfully.";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    _logger.LogError("Error updating user profile: {Error}", error.Description);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user profile");
                ModelState.AddModelError(string.Empty, "An error occurred while updating your profile.");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProfileViewModel model)
        {
            _logger.LogInformation("Profile update attempt received for model: {@Model}", model);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid: {@ModelState}", ModelState);
                return View("Index", model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogWarning("User not found during profile update");
                return NotFound();
            }

            try
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Department = model.Department;
                user.Position = model.Position;
                user.Qualification = model.Qualification;
                user.University = model.University;
                user.UpdatedAt = DateTime.UtcNow;
                user.UpdateProfileCompletionStatus();

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User {UserId} updated their profile successfully", user.Id);
                    TempData["StatusMessage"] = "Your profile has been updated successfully.";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    _logger.LogError("Error updating user profile: {Error}", error.Description);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user profile");
                ModelState.AddModelError(string.Empty, "An error occurred while updating your profile.");
            }

            return View("Index", model);
        }
    }
}
