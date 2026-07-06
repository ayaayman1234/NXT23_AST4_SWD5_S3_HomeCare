using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels.Complaint;

namespace NursingCarePlatform.Web.Controllers
{
    [Authorize]
    public class ComplaintController : Controller
    {
        private readonly IComplaintService _complaintService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ComplaintController(
            IComplaintService complaintService,
            UserManager<ApplicationUser> userManager)
        {
            _complaintService = complaintService;
            _userManager = userManager;
        }

        // =====================================
        // Create Complaint
        // =====================================

        [Authorize(Roles = "Patient")]
        [HttpGet]
        public IActionResult Create()
        {
            return View(new ComplaintViewModel());
        }

        [Authorize(Roles = "Patient")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ComplaintViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var result = await _complaintService.CreateComplaintAsync(user.Id, model);

            TempData[result.Success ? "Success" : "Error"] = result.Message;

            if (!result.Success)
                return View(model);

            return RedirectToAction(nameof(Create));
        }

        // =====================================
        // Admin - All Complaints
        // =====================================

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var complaints = await _complaintService.GetAllComplaintsAsync();

            return View(complaints);
        }

        // =====================================
        // Resolve
        // =====================================

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Resolve(int id)
        {
            var result = await _complaintService.ResolveComplaintAsync(id);

            TempData[result.Success ? "Success" : "Error"] = result.Message;

            return RedirectToAction(nameof(Index));
        }

        // =====================================
        // Close
        // =====================================

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Close(int id)
        {
            var result = await _complaintService.CloseComplaintAsync(id);

            TempData[result.Success ? "Success" : "Error"] = result.Message;

            return RedirectToAction(nameof(Index));
        }
    }
}