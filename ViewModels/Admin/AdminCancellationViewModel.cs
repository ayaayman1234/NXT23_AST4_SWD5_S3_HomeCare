namespace NursingCarePlatform.Web.ViewModels.Admin
{
    public class AdminCancellationViewModel
    {
        public int Id { get; set; }

        public int CareRequestId { get; set; }

        public string RequestedBy { get; set; } = "";

        public string RequestedByType { get; set; } = "";

        public string Reason { get; set; } = "";

        public decimal Fee { get; set; }

        public DateTime RequestedAt { get; set; }

        public string Status { get; set; } = "";
    }
}