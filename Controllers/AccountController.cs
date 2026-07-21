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
        private readonly ISubscriptionService _subscriptionService;

        public AccountController(
            IAccountService accountService,
            UserManager<ApplicationUser> userManager,
            ISubscriptionService subscriptionService)
        {
            _accountService = accountService;
            _userManager = userManager;
            _subscriptionService = subscriptionService;
        }

        // =========================
        // Register
        // =========================

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // ── Nurses must pick a plan first ─────────────────────────────────
            if (model.Role == "Nurse")
            {
                // Stash the form data in TempData and redirect to plan picker
                TempData["Reg_FirstName"]     = model.FirstName;
                TempData["Reg_LastName"]      = model.LastName;
                TempData["Reg_Email"]         = model.Email;
                TempData["Reg_Password"]      = model.Password;
                TempData["Reg_Phone"]         = model.PhoneNumber;
                TempData["Reg_Address"]       = model.Address;
                TempData["Reg_City"]          = model.City;
                TempData["Reg_Governorate"]   = model.Governorate;
                TempData["Reg_Gender"]        = model.Gender;
                TempData["Reg_Age"]           = model.Age;
                TempData["Reg_YearsExp"]      = model.YearsExperience;
                TempData["Reg_Specialization"]= model.Specialization;
                return RedirectToAction(nameof(ChoosePlan));
            }
            // ─────────────────────────────────────────────────────────────────

            var result = await _accountService.RegisterAsync(model);

            if (!result.Success)
            {
                if (result.Errors.Any())
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error);
                else
                    ModelState.AddModelError("", result.Message);

                return View(model);
            }

            TempData["Success"] = result.Message;
            return RedirectToAction(nameof(Login));
        }

        // =========================
        // Choose Plan (Nurse only)
        // =========================

        [HttpGet]
        public async Task<IActionResult> ChoosePlan()
        {
            // Make sure we have registration data in TempData
            if (TempData["Reg_Email"] == null)
                return RedirectToAction(nameof(Register));

            // Keep TempData alive for POST
            TempData.Keep();

            var plans = await _subscriptionService.GetAllPlansAsync();
            ViewBag.Plans = plans.Where(p => p.IsActive).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChoosePlan(int selectedPlanId)
        {
            // Rebuild the model from TempData
            var model = new RegisterViewModel
            {
                Role           = "Nurse",
                FirstName      = TempData["Reg_FirstName"]?.ToString() ?? "",
                LastName       = TempData["Reg_LastName"]?.ToString()  ?? "",
                Email          = TempData["Reg_Email"]?.ToString()     ?? "",
                Password       = TempData["Reg_Password"]?.ToString()  ?? "",
                ConfirmPassword= TempData["Reg_Password"]?.ToString()  ?? "",
                PhoneNumber    = TempData["Reg_Phone"]?.ToString()     ?? "",
                Address        = TempData["Reg_Address"]?.ToString()   ?? "",
                City           = TempData["Reg_City"]?.ToString()      ?? "",
                Governorate    = TempData["Reg_Governorate"]?.ToString()?? "",
                Gender         = TempData["Reg_Gender"]?.ToString()    ?? "",
                Age            = TempData["Reg_Age"] is int a ? a : 0,
                YearsExperience= TempData["Reg_YearsExp"] is int y ? y : 0,
                Specialization = TempData["Reg_Specialization"]?.ToString(),
                SelectedPlanId = selectedPlanId
            };

            var result = await _accountService.RegisterAsync(model);

            if (!result.Success)
            {
                if (result.Errors.Any())
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error);
                else
                    ModelState.AddModelError("", result.Message);

                // Re-show plan picker with error
                var plans = await _subscriptionService.GetAllPlansAsync();
                ViewBag.Plans = plans.Where(p => p.IsActive).ToList();
                return View();
            }

            TempData["Success"] = "Registration completed! Please log in.";
            return RedirectToAction(nameof(Login));
        }

        // =========================
        // Login
        // =========================

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
                return RedirectToAction("Dashboard", "Admin");

            if (await _accountService.IsInRoleAsync(user, "Nurse"))
                return RedirectToAction("Dashboard", "Nurse");

            if (await _accountService.IsInRoleAsync(user, "Patient"))
                return RedirectToAction("Dashboard", "Patient");

            return RedirectToAction("Index", "Home");
        }

        // =========================
        // Institution Coming Soon
        // =========================

        [HttpGet]
        public IActionResult InstitutionComingSoon()
        {
            return View();
        }

        // =========================
        // Logout
        // =========================

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}