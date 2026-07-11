using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels.Complaint;

namespace NursingCarePlatform.Web.Controllers
{
    [Authorize(Roles = "Patient")]
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

        //==============================
        // Create
        //==============================

        [HttpGet]
        public IActionResult Create(int? nurseId)
        {
            return View(new CreateComplaintViewModel
            {
                NurseId = nurseId
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateComplaintViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);

            var result = await _complaintService.CreateComplaintAsync(user.Id, model);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(MyComplaints));
        }

        //==============================
        // My Complaints
        //==============================

        public async Task<IActionResult> MyComplaints()
        {
            var user = await _userManager.GetUserAsync(User);

            var complaints =
                await _complaintService.GetPatientComplaintsAsync(user.Id);

            return View(complaints);
        }

        //==============================
        // Details
        //==============================

        public async Task<IActionResult> Details(int id)
        {
            var complaint =
                await _complaintService.GetComplaintDetailsAsync(id);

            if (complaint == null)
                return NotFound();

            return View(complaint);
        }
    }
}