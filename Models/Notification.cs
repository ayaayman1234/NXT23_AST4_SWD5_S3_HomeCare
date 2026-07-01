namespace NursingCarePlatform.Web.Models
{
    public class Notification
    {
        public int Id { get; set; }

        public int SenderId { get; set; }

        public int ReceiverId { get; set; }

        public string SenderType { get; set; } = string.Empty;

        public string ReceiverType { get; set; } = string.Empty;

        public string MessageContent { get; set; } = string.Empty;

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; }
    }

}