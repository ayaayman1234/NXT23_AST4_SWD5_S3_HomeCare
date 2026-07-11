using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels.SOS;

namespace NursingCarePlatform.Web.Controllers
{
    [Authorize]
    public class SOSController : Controller
    {
        private readonly ISOSService _sosService;
        private readonly UserManager<ApplicationUser> _userManager;

        public SOSController(
            ISOSService sosService,
            UserManager<ApplicationUser> userManager)
        {
            _sosService = sosService;
            _userManager = userManager;
        }

        // ===========================
        // History
        // ===========================

        public async Task<IActionResult> Index()
        {
            var list = await _sosService.GetAllSOSAsync();

            return View(list);
        }

        // ===========================
        // Details
        // ===========================

        public async Task<IActionResult> Details(int id)
        {
            var model = await _sosService.GetSOSDetailsAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        // ===========================
        // Create
        // ===========================

        [HttpGet]
        public IActionResult Create(int? careRequestId)
        {
            return View(new CreateSOSViewModel
            {
                CareRequestId = careRequestId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSOSViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var result = await _sosService.CreateSOSAsync(user.Id, model);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(Index));
        }
    }
}