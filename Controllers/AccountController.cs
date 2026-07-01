using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels.Account;

namespace NursingCarePlatform.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(
    IAccountService accountService,
    UserManager<ApplicationUser> userManager)
        {
            _accountService = accountService;
            _userManager = userManager;
        }

        // ================= Register =================

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _accountService.RegisterAsync(model);

            if (!result.Success)
            {
                if (result.Errors.Any())
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error);
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                }

                return View(model);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(Login));
        }

        // ================= Login =================

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _accountService.LoginAsync(model);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }

            var user = await _accountService.GetUserByEmailAsync(model.Email);

            if (user == null)
                return RedirectToAction("Index", "Home");

            if (await _accountService.IsInRoleAsync(user, "Admin"))
                return RedirectToAction("Index", "Admin");

            if (await _accountService.IsInRoleAsync(user, "Nurse"))
                return RedirectToAction("Index", "Nurse");

            if (await _accountService.IsInRoleAsync(user, "Patient"))
                return RedirectToAction("Index", "Patient");

            return RedirectToAction("Index", "Home");
        }

        // ================= Logout =================

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();

            return RedirectToAction(nameof(Login));
        }
    }
}