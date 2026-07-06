using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Services.Interfaces;

namespace NursingCarePlatform.Web.Controllers
{
    [Authorize(Roles = "Nurse")]
    public class WorkHistoryController : Controller
    {
        private readonly IWorkHistoryService _workHistoryService;
        private readonly UserManager<ApplicationUser> _userManager;

        public WorkHistoryController(
            IWorkHistoryService workHistoryService,
            UserManager<ApplicationUser> userManager)
        {
            _workHistoryService = workHistoryService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var history =
                await _workHistoryService.GetHistoryAsync(user.Id);

            return View(history);
        }
    }
}