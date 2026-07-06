using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Services.Interfaces;

namespace NursingCarePlatform.Web.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationController(
            INotificationService notificationService,
            UserManager<ApplicationUser> userManager)
        {
            _notificationService = notificationService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var notifications =
                await _notificationService.GetUserNotificationsAsync(user.Id);

            return View(notifications);
        }

        public async Task<IActionResult> Read(int id)
        {
            await _notificationService.MarkAsReadAsync(id);

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ReadAll()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var notifications =
                await _notificationService.GetUserNotificationsAsync(user.Id);

            foreach (var item in notifications.Where(x => !x.IsRead))
            {
                await _notificationService.MarkAsReadAsync(item.Id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}