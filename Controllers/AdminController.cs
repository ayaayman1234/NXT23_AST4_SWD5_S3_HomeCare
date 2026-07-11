using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels.Admin;

namespace NursingCarePlatform.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISubscriptionService _subscriptionService;

        public AdminController(
            IAdminService adminService,
            UserManager<ApplicationUser> userManager,
            ISubscriptionService subscriptionService)
        {
            _adminService = adminService;
            _userManager = userManager;
            _subscriptionService = subscriptionService;
        }

        //==================================================
        // Dashboard
        //==================================================

        public async Task<IActionResult> Index()
        {
            return RedirectToAction(nameof(Dashboard));
        }

        public async Task<IActionResult> Dashboard()
        {
            var model = await _adminService.GetDashboardAsync();

            return View("Index", model);
        }

        //==================================================
        // Nurses
        //==================================================

        public async Task<IActionResult> Nurses()
        {
            var model = await _adminService.GetAllNursesAsync();

            return View(model);
        }

        //==================================================
        // Nurse Details
        //==================================================

        public async Task<IActionResult> NurseDetails(int id)
        {
            var model = await _adminService.GetNurseDetailsAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        //==================================================
        // Approve Nurse
        //==================================================

        public async Task<IActionResult> ApproveNurse(int id)
        {
            var result = await _adminService.ApproveNurseAsync(id);

            if (result.Success)
                TempData["Success"] = result.Message;
            else
                TempData["Error"] = result.Message;

            return RedirectToAction(nameof(Nurses));
        }

        //==================================================
        // Reject Nurse
        //==================================================

        public async Task<IActionResult> RejectNurse(int id)
        {
            await _adminService.RejectNurseAsync(id);

            return RedirectToAction(nameof(Nurses));
        }

        //==================================================
        // Block Nurse
        //==================================================

        public async Task<IActionResult> BlockNurse(int id)
        {
            await _adminService.BlockNurseAsync(id);

            return RedirectToAction(nameof(Nurses));
        }

        //==================================================
        // UnBlock Nurse
        //==================================================

        public async Task<IActionResult> UnBlockNurse(int id)
        {
            await _adminService.UnBlockNurseAsync(id);

            return RedirectToAction(nameof(Nurses));
        }

        //==================================================
        // Delete Nurse (Business Logic enforced)
        //==================================================

        public async Task<IActionResult> DeleteNurse(int id)
        {
            var result = await _adminService.DeleteNurseAsync(id);

            if (result.Success)
                TempData["Success"] = result.Message;
            else
                TempData["Error"] = result.Message;

            return RedirectToAction(nameof(Nurses));
        }

        //====================================================
        // Nurse Documents
        //====================================================

        public async Task<IActionResult> NurseDocuments(int id)
        {
            var model = await _adminService.GetNurseDocumentsAsync(id);

            return View(model);
        }
        //====================================================
        // Approve Document
        //====================================================

        public async Task<IActionResult> ApproveDocument(int id)
        {
            await _adminService.ApproveDocumentAsync(id);

            return RedirectToAction(nameof(Index));
        }
        //====================================================
        // Reject Document
        //====================================================

        public async Task<IActionResult> RejectDocument(int id)
        {
            await _adminService.RejectDocumentAsync(id);

            return RedirectToAction(nameof(Index));
        }

        //==================================================
        // Patients
        //==================================================

        public async Task<IActionResult> Patients()
        {
            var model = await _adminService.GetAllPatientsAsync();

            return View(model);
        }

        //==================================================
        // Patient Details
        //==================================================

        public async Task<IActionResult> PatientDetails(int id)
        {
            var model = await _adminService.GetPatientDetailsAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        //==================================================
        // Block Patient
        //==================================================

        public async Task<IActionResult> BlockPatient(int id)
        {
            var result = await _adminService.BlockPatientAsync(id);

            if (result.Success)
                TempData["Success"] = result.Message;
            else
                TempData["Error"] = result.Message;

            return RedirectToAction(nameof(Patients));
        }

        //==================================================
        // UnBlock Patient
        //==================================================

        public async Task<IActionResult> UnBlockPatient(int id)
        {
            var result = await _adminService.UnBlockPatientAsync(id);

            if (result.Success)
                TempData["Success"] = result.Message;
            else
                TempData["Error"] = result.Message;

            return RedirectToAction(nameof(Patients));
        }

        //==================================================
        // Delete Patient
        //==================================================

        public async Task<IActionResult> DeletePatient(int id)
        {
            await _adminService.DeletePatientAsync(id);

            return RedirectToAction(nameof(Patients));
        }


        //==================================================
        // Care Requests
        //==================================================

        public async Task<IActionResult> CareRequests()
        {
            var model = await _adminService.GetAllCareRequestsAsync();

            return View(model);
        }
        //==================================================
        // Care Request Details
        //==================================================

        public async Task<IActionResult> CareRequestDetails(int id)
        {
            var model = await _adminService.GetCareRequestDetailsAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        //==================================================
        // Delete Care Request
        //==================================================

        public async Task<IActionResult> DeleteCareRequest(int id)
        {
            await _adminService.DeleteCareRequestAsync(id);

            return RedirectToAction(nameof(CareRequests));
        }

        //==================================================
        // Complaints
        //==================================================

        public async Task<IActionResult> Complaints()
        {
            var model = await _adminService.GetAllComplaintsAsync();

            return View(model);
        }

        //==================================================
        // Complaint Details
        //==================================================

        public async Task<IActionResult> ComplaintDetails(int id)
        {
            var model = await _adminService.GetComplaintAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        //==================================================
        // Resolve Complaint
        //==================================================

        [HttpPost]
        public async Task<IActionResult> ResolveComplaint(
            int id,
            string? adminNotes)
        {
            await _adminService.ResolveComplaintAsync(id, adminNotes);

            return RedirectToAction(nameof(Complaints));
        }

        //==================================================
        // Reject Complaint
        //==================================================

        [HttpPost]
        public async Task<IActionResult> RejectComplaint(
            int id,
            string? adminNotes)
        {
            await _adminService.RejectComplaintAsync(id, adminNotes);

            return RedirectToAction(nameof(Complaints));
        }
        //=========================================
        // SOS
        //=========================================

        public async Task<IActionResult> SOS()
        {
            var model = await _adminService.GetAllSOSAsync();

            return View(model);
        }
        //=========================================
        // Resolve SOS
        //=========================================

        public async Task<IActionResult> ResolveSOS(int id)
        {
            await _adminService.ResolveSOSAsync(id);

            return RedirectToAction(nameof(SOS));
        }
        //==========================================
        // Cancellation Requests
        //==========================================

        public async Task<IActionResult> Cancellations()
        {
            var model = await _adminService.GetAllCancellationsAsync();

            return View(model);
        }
        //==========================================
        // Approve Cancellation
        //==========================================

        public async Task<IActionResult> ApproveCancellation(int id)
        {
            await _adminService.ApproveCancellationAsync(id);

            return RedirectToAction(nameof(Cancellations));
        }
        //==========================================
        // Reject Cancellation
        //==========================================

        public async Task<IActionResult> RejectCancellation(int id)
        {
            await _adminService.RejectCancellationAsync(id);

            return RedirectToAction(nameof(Cancellations));
        }
        //======================================
        // Payments
        //======================================

        public async Task<IActionResult> Payments()
        {
            var model = await _adminService.GetAllPaymentsAsync();

            return View(model);
        }
        //======================================
        // Payment Details
        //======================================

        public async Task<IActionResult> PaymentDetails(int id)
        {
            var model = await _adminService.GetPaymentDetailsAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        //======================================
        // Ratings
        //======================================

        public async Task<IActionResult> Ratings()
        {
            var model = await _adminService.GetAllRatingsAsync();

            return View(model);
        }

        //======================================
        // Reports
        //======================================

        public async Task<IActionResult> Reports()
        {
            var model = await _adminService.GetReportAsync();

            return View(model);
        }

        //======================================
        // Admin Profile
        //======================================

        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) return NotFound();

            var model = await _adminService.GetProfileAsync(user.Id);

            if (model == null) return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(AdminProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);

            if (user == null) return NotFound();

            var result = await _adminService.UpdateProfileAsync(user.Id, model);

            if (result.Success)
                TempData["Success"] = "Profile updated successfully.";
            else
                TempData["Error"] = result.Message;

            return RedirectToAction(nameof(Profile));
        }

        //======================================
        // Change Password
        //======================================

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);

            if (user == null) return NotFound();

            var result = await _userManager.ChangePasswordAsync(
                user,
                model.CurrentPassword,
                model.NewPassword);

            if (result.Succeeded)
            {
                TempData["Success"] = "Password changed successfully.";
                return RedirectToAction(nameof(Profile));
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        //======================================
        // Notifications
        //======================================

        public async Task<IActionResult> Notifications()
        {
            var model = await _adminService.GetAllNotificationsAsync();

            return View(model);
        }

        public async Task<IActionResult> MarkNotificationRead(int id)
        {
            await _adminService.MarkNotificationReadAsync(id);

            return RedirectToAction(nameof(Notifications));
        }

        //======================================
        // GAP 1 – Subscription Plans
        //======================================

        public async Task<IActionResult> SubscriptionPlans()
        {
            var model = await _subscriptionService.GetAllPlansAsync();
            return View(model);
        }

        [HttpGet]
        public IActionResult CreateSubscriptionPlan()
        {
            return View(new CreateSubscriptionPlanViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSubscriptionPlan(CreateSubscriptionPlanViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _subscriptionService.CreatePlanAsync(model);

            if (result.Success)
                TempData["Success"] = result.Message;
            else
                TempData["Error"] = result.Message;

            return RedirectToAction(nameof(SubscriptionPlans));
        }

        [HttpGet]
        public async Task<IActionResult> EditSubscriptionPlan(int id)
        {
            var plan = await _subscriptionService.GetPlanByIdAsync(id);

            if (plan == null) return NotFound();

            var model = new CreateSubscriptionPlanViewModel
            {
                Name = plan.Name,
                Description = plan.Description,
                MonthlyFee = plan.MonthlyFee,
                CommissionRate = plan.CommissionRate,
                IsActive = plan.IsActive
            };

            ViewBag.PlanId = id;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSubscriptionPlan(int id, CreateSubscriptionPlanViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PlanId = id;
                return View(model);
            }

            var result = await _subscriptionService.UpdatePlanAsync(id, model);

            TempData[result.Success ? "Success" : "Error"] = result.Message;

            return RedirectToAction(nameof(SubscriptionPlans));
        }

        public async Task<IActionResult> ToggleSubscriptionPlan(int id)
        {
            var result = await _subscriptionService.TogglePlanStatusAsync(id);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction(nameof(SubscriptionPlans));
        }

        public async Task<IActionResult> DeleteSubscriptionPlan(int id)
        {
            var result = await _subscriptionService.DeletePlanAsync(id);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction(nameof(SubscriptionPlans));
        }

        //======================================
        // Nurse Subscription Management
        //======================================

        public async Task<IActionResult> NurseSubscriptions()
        {
            var model = await _subscriptionService.GetAllNurseSubscriptionsAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AssignSubscription(int nurseId)
        {
            ViewBag.NurseId = nurseId;
            ViewBag.Plans = await _subscriptionService.GetAllPlansAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignSubscription(
            int nurseId, int planId, DateTime startDate, DateTime endDate)
        {
            var result = await _subscriptionService.AssignPlanToNurseAsync(nurseId, planId, startDate, endDate);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction(nameof(NurseSubscriptions));
        }

        public async Task<IActionResult> CancelSubscription(int id)
        {
            var result = await _subscriptionService.CancelSubscriptionAsync(id);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction(nameof(NurseSubscriptions));
        }
    }
}