namespace NursingCarePlatform.Web.Models
{
    public class SOSEvent
    {
        public int Id { get; set; }

        public int TriggeredByUserId { get; set; }

        public Patient TriggeredByUser { get; set; } = null!;

        public int? CareRequestId { get; set; }

        public CareRequest? CareRequest { get; set; }

        public string Location { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string SOSStatus { get; set; } = "Pending";
        public string? AdminNotes { get; set; }
    }
}