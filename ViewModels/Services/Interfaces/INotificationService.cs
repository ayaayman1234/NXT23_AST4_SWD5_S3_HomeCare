using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.ViewModels.Notification;

namespace NursingCarePlatform.Web.Services.Interfaces
{
    public interface INotificationService
    {
        Task CreateAsync(
    string userId,
    string title,
    string message,
    string notificationType);

        Task<List<NotificationViewModel>> GetUserNotificationsAsync(string userId);

        Task MarkAsReadAsync(int id);
    }
}