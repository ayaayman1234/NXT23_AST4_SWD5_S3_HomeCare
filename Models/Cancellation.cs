namespace NursingCarePlatform.Web.Models
{

    public class Cancellation
    {
        public int Id { get; set; }

        public int CareRequestId { get; set; }

        public string Reason { get; set; } = string.Empty;

        public DateTime CancelledAt { get; set; }

        public decimal Fee { get; set; }

        public CareRequest CareRequest { get; set; } = null!;
    }
}