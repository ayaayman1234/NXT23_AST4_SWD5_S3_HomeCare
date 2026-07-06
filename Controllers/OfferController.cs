using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels.Offer;

namespace NursingCarePlatform.Web.Controllers
{
    
    public class OfferController : Controller
    {
        private readonly IOfferService _offerService;
        private readonly UserManager<ApplicationUser> _userManager;

        public OfferController(
            IOfferService offerService,
            UserManager<ApplicationUser> userManager)
        {
            _offerService = offerService;
            _userManager = userManager;
        }

        // ===============================
        // GET
        // ===============================

        [HttpGet]
        public async Task<IActionResult> SendOffer(int id)
        {
            var model = await _offerService.GetRequestAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        // ===============================
        // POST
        // ===============================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendOffer(SendOfferViewModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var item in ModelState)
                {
                    foreach (var error in item.Value.Errors)
                    {
                        Console.WriteLine($"{item.Key} ==> {error.ErrorMessage}");
                    }
                }

                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var result = await _offerService.SendOfferAsync(user.Id, model);

            Console.WriteLine($"Success = {result.Success}");
            Console.WriteLine($"Message = {result.Message}");

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction("AvailableRequests", "Nurse");
        }
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> AcceptOffer(int id)
        {
            await _offerService.AcceptOfferAsync(id);

            TempData["Success"] = "Offer accepted successfully.";

            return RedirectToAction("MyRequests", "Patient");
        }

        public async Task<IActionResult> RejectOffer(int id)
        {
            await _offerService.RejectOfferAsync(id);

            TempData["Success"] = "Offer rejected.";

            return RedirectToAction("MyRequests", "Patient");
        }
    }
}