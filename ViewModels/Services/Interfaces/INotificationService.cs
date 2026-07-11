using NursingCarePlatform.Web.ViewModels.Notification;

namespace NursingCarePlatform.Web.Services.Interfaces
{
    public interface INotificationService
    {
        // ==========================================
        // Create Notification
        // ==========================================
        Task CreateAsync(
            int receiverId,
            string receiverType,
            string title,
            string message,
            string notificationType);

        // ==========================================
        // Get User Notifications
        // ==========================================
        Task<List<NotificationViewModel>> GetUserNotificationsAsync(string userId);

        // ==========================================
        // Mark Notification As Read
        // ==========================================
        Task MarkAsReadAsync(int id);
    }
}