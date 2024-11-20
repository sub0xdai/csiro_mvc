using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using csiro_mvc.Models;
using csiro_mvc.ViewModels;
using Microsoft.Extensions.Logging;

namespace csiro_mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { 
                    UserName = model.Email, 
                    Email = model.Email,
                    FirstName = "",
                    LastName = "",
                    Department = "",
                    Position = "",
                    Qualification = "",
                    University = ""
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Check if this is the first user
                    if (_userManager.Users.Count() == 1)
                    {
                        // Create Admin role if it doesn't exist
                        if (!await _roleManager.RoleExistsAsync("Admin"))
                        {
                            await _roleManager.CreateAsync(new IdentityRole("Admin"));
                        }
                        // Assign Admin role to the first user
                        await _userManager.AddToRoleAsync(user, "Admin");
                    }
                    else
                    {
                        // Assign default Applicant role to new users
                        if (!await _roleManager.RoleExistsAsync("Applicant"))
                        {
                            await _roleManager.CreateAsync(new IdentityRole("Applicant"));
                        }
                        await _userManager.AddToRoleAsync(user, "Applicant");
                    }

                    await _signInManager.SignInAsync(user, isPersistent: true);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    _logger.LogWarning("Registration error: {Error}", error.Description);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user != null)
                    {
                        // Get user's roles
                        var roles = await _userManager.GetRolesAsync(user);
                        
                        // Sign out first to clear any existing claims
                        await _signInManager.SignOutAsync();
                        
                        // Sign in again with roles
                        await _signInManager.SignInAsync(user, model.RememberMe);
                        
                        _logger.LogInformation("User {Email} logged in successfully with roles: {Roles}", 
                            user.Email, string.Join(", ", roles));
                            
                        return RedirectToAction("Index", "Home");
                    }
                }
                
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                _logger.LogWarning("Invalid login attempt for user: {Email}", model.Email);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                _logger.LogInformation("User logged out: {Email}", user.Email);
            }
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
