using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NursingCarePlatform.Web.Services.Interfaces;

namespace NursingCarePlatform.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly INotificationService _notificationService;

        public AdminController(
            IAdminService adminService,
            INotificationService notificationService)
        {
            _adminService = adminService;
            _notificationService = notificationService;
        }

        // =====================================
        // Dashboard
        // =====================================

        public async Task<IActionResult> Dashboard()
        {
            ViewBag.TotalPatients = await _adminService.GetTotalPatientsAsync();
            ViewBag.TotalNurses = await _adminService.GetTotalNursesAsync();
            ViewBag.PendingNurses = await _adminService.GetPendingNursesCountAsync();

            return View();
        }

        // =====================================
        // Pending Nurses
        // =====================================

        public async Task<IActionResult> PendingNurses()
        {
            var nurses = await _adminService.GetPendingNursesAsync();

            return View(nurses);
        }

        // =====================================
        // Approve Nurse
        // =====================================

        public async Task<IActionResult> ApproveNurse(int id)
        {
            var nurse = await _adminService.GetNurseByIdAsync(id);

            if (nurse == null)
            {
                TempData["Error"] = "Nurse not found.";

                return RedirectToAction(nameof(PendingNurses));
            }

            var result = await _adminService.ApproveNurseAsync(id);

            if (result.Success)
            {
                await _notificationService.CreateAsync(
    nurse.UserId,
    "Account Approved",
    "Congratulations! Your nurse account has been approved by the administrator.",
    "AccountApproved"
);
            }

            TempData[result.Success ? "Success" : "Error"] = result.Message;

            return RedirectToAction(nameof(PendingNurses));
        }

        // =====================================
        // Reject Nurse
        // =====================================

        public async Task<IActionResult> RejectNurse(int id)
        {
            var result = await _adminService.RejectNurseAsync(id);

            TempData[result.Success ? "Success" : "Error"] = result.Message;

            return RedirectToAction(nameof(PendingNurses));
        }

        // =====================================
        // Users
        // =====================================

        public async Task<IActionResult> Users()
        {
            var users = await _adminService.GetAllUsersAsync();

            return View(users);
        }

        // =====================================
        // Block User
        // =====================================

        public async Task<IActionResult> BlockUser(string id)
        {
            var result = await _adminService.BlockUserAsync(id);

            TempData[result.Success ? "Success" : "Error"] = result.Message;

            return RedirectToAction(nameof(Users));
        }

        // =====================================
        // Unblock User
        // =====================================

        public async Task<IActionResult> UnblockUser(string id)
        {
            var result = await _adminService.UnblockUserAsync(id);

            TempData[result.Success ? "Success" : "Error"] = result.Message;

            return RedirectToAction(nameof(Users));
        }
    }
}