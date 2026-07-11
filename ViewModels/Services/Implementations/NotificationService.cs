using Microsoft.EntityFrameworkCore;
using NursingCarePlatform.Web.Data;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels.Notification;

namespace NursingCarePlatform.Web.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly NursingDbContext _context;

        public NotificationService(NursingDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // Create Notification
        // ==========================================

        public async Task CreateAsync(
            int receiverId,
            string receiverType,
            string title,
            string message,
            string notificationType)
        {
            var notification = new Notification
            {
                SenderId = 0,
                SenderType = "System",

                ReceiverId = receiverId,
                ReceiverType = receiverType,

                Title = title,
                Message = message,
                NotificationType = notificationType,

                IsRead = false,
                CreatedAt = DateTime.Now
            };

            _context.Notifications.Add(notification);

            await _context.SaveChangesAsync();
        }

        // ==========================================
        // User Notifications
        // ==========================================

        public async Task<List<NotificationViewModel>> GetUserNotificationsAsync(string userId)
        {
            int receiverId = 0;
            string receiverType = "";

            var patient = await _context.Patients
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (patient != null)
            {
                receiverId = patient.Id;
                receiverType = "Patient";
            }
            else
            {
                var nurse = await _context.Nurses
                    .FirstOrDefaultAsync(x => x.UserId == userId);

                if (nurse != null)
                {
                    receiverId = nurse.Id;
                    receiverType = "Nurse";
                }
                else
                {
                    var admin = await _context.Admins
                        .FirstOrDefaultAsync(x => x.UserId == userId);

                    if (admin != null)
                    {
                        receiverId = admin.Id;
                        receiverType = "Admin";
                    }
                }
            }

            return await _context.Notifications
                .Where(x => x.ReceiverId == receiverId && x.ReceiverType == receiverType)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new NotificationViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Message = x.Message,
                    NotificationType = x.NotificationType,
                    IsRead = x.IsRead,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();
        }

        // ==========================================
        // Mark As Read
        // ==========================================

        public async Task MarkAsReadAsync(int id)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(x => x.Id == id);

            if (notification == null)
                return;

            notification.IsRead = true;

            await _context.SaveChangesAsync();
        }
    }
}