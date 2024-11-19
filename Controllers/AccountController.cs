using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using csiro_mvc.Models;
using csiro_mvc.ViewModels;

namespace csiro_mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View("Auth", new AuthViewModel { IsRegistering = true });
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("Auth", new AuthViewModel { IsRegistering = false });
        }

        [HttpPost]
        public async Task<IActionResult> Register(AuthViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { 
                    UserName = model.Email, 
                    Email = model.Email,
                    FirstName = "",  // These can be updated later
                    LastName = "",
                    Qualification = "",
                    University = "",
                    CoverLetter = "",
                    CVPath = ""
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            model.IsRegistering = true;
            return View("Auth", model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(AuthViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            model.IsRegistering = false;
            return View("Auth", model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
