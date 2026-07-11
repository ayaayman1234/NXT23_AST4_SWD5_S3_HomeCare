namespace NursingCarePlatform.Web.ViewModels.Cancellation
{
    public class CancellationHistoryViewModel
    {
        public int Id { get; set; }

        public int CareRequestId { get; set; }

        public string RequestedBy { get; set; } = "";

        public string Reason { get; set; } = "";

        public decimal Fee { get; set; }

        public string Status { get; set; } = "";

        public DateTime RequestedAt { get; set; }
    }
}