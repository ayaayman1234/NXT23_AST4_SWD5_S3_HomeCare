namespace NursingCarePlatform.Web.Models
{
    public class Cancellation
    {
        public int Id { get; set; }

        public int CareRequestId { get; set; }

        public int RequestedById { get; set; }

        public string RequestedByType { get; set; } = string.Empty;

        public string Reason { get; set; } = string.Empty;

        public decimal Fee { get; set; }

        public DateTime RequestedAt { get; set; }

        public string Status { get; set; } = "Pending";

        public CareRequest CareRequest { get; set; } = null!;
        public string? AdminNotes { get; set; }
    }
}