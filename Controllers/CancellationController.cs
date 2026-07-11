using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels.Cancellation;

namespace NursingCarePlatform.Web.Controllers
{
    [Authorize]
    public class CancellationController : Controller
    {
        private readonly ICancellationService _cancellationService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CancellationController(
            ICancellationService cancellationService,
            UserManager<ApplicationUser> userManager)
        {
            _cancellationService = cancellationService;
            _userManager = userManager;
        }

        // ==========================================
        // Create
        // ==========================================

        [HttpGet]
        public IActionResult Create(int careRequestId)
        {
            var model = new CreateCancellationViewModel
            {
                CareRequestId = careRequestId
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCancellationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var result = await _cancellationService.CreateCancellationAsync(
                user.Id,
                model);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(History));
        }

        // ==========================================
        // History
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> History()
        {
            var list = await _cancellationService.GetAllAsync();

            return View(list);
        }
    }
}