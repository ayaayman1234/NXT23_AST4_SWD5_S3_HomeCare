namespace NursingCarePlatform.Web.ViewModels.Admin
{
    public class AdminNotificationViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public string Message { get; set; } = "";

        public string NotificationType { get; set; } = "";

        public string ReceiverType { get; set; } = "";

        public int ReceiverId { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
