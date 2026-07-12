using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Services.Interfaces;

namespace NursingCarePlatform.Web.Controllers
{
    [Authorize(Roles = "Nurse")]
    public class AssignmentController : Controller
    {
        private readonly IAssignmentService _assignmentService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotificationService _notificationService; 

        public AssignmentController(
            IAssignmentService assignmentService,
            UserManager<ApplicationUser> userManager,
            INotificationService notificationService)
        {
            _assignmentService = assignmentService;
            _userManager = userManager;
            _notificationService = notificationService;
           
        }

        // ===========================
        // My Assignments
        // ===========================

        public async Task<IActionResult> MyAssignments()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var assignments =
                await _assignmentService.GetNurseAssignmentsAsync(user.Id);

            return View(assignments);
        }
        // ===========================
        // Work History
        // ===========================

        public async Task<IActionResult> WorkHistory()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var history = await _assignmentService.GetWorkHistoryAsync(user.Id);

            return View(history);
        }

        // ===========================
        // Details
        // ===========================

        public async Task<IActionResult> Details(int id)
        {
            var assignment = await _assignmentService.GetAssignmentAsync(id);

            if (assignment == null)
                return NotFound();

            return View(assignment);
        }

        // ===========================
        // Complete
        // ===========================

        public async Task<IActionResult> Complete(int id)
        {
            var result = await _assignmentService.CompleteAssignmentAsync(id);

            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction(nameof(MyAssignments));
            }

            var assignment = await _assignmentService.GetAssignmentAsync(id);

            if (assignment == null)
            {
                TempData["Error"] = "Assignment not found.";
                return RedirectToAction(nameof(MyAssignments));
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(
                "Create",
                "Payment",
                new
                {
                    careRequestId = assignment.CareRequestId
                });
        }

    }
}