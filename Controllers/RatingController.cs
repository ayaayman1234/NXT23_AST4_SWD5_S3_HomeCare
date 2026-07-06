using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels.Rating;

namespace NursingCarePlatform.Web.Controllers
{
    [Authorize(Roles = "Patient")]
    public class RatingController : Controller
    {
        private readonly IRatingService _ratingService;
        private readonly UserManager<ApplicationUser> _userManager;

        public RatingController(
            IRatingService ratingService,
            UserManager<ApplicationUser> userManager)
        {
            _ratingService = ratingService;
            _userManager = userManager;
        }

        // ==========================================
        // Create Rating
        // ==========================================

        [HttpGet]
        public IActionResult Create(int careRequestId, int nurseId)
        {
            var model = new CreateRatingViewModel
            {
                CareRequestId = careRequestId,
                NurseId = nurseId
            };

            return View(model);
        }

        // ==========================================
        // Save Rating
        // ==========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRatingViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var result = await _ratingService.AddRatingAsync(user.Id, model);

            TempData[result.Success ? "Success" : "Error"] = result.Message;

            if (result.Success)
                return RedirectToAction("MyRequests", "Patient");

            return View(model);
        }
    }
}