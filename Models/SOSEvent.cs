namespace NursingCarePlatform.Web.Models
{
    public class SOSEvent
    {
        public int Id { get; set; }

        public int TriggeredByUserId { get; set; }

        public int? CareRequestId { get; set; }

        public string Location { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public string SOSStatus { get; set; } = string.Empty;
    }
}