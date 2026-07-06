namespace NursingCarePlatform.Web.ViewModels.CareRequest
{
    public class CareRequestListViewModel
    {
        public int Id { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public string ServiceName { get; set; } = string.Empty;

        public DateTime PreferredDate { get; set; }

        public decimal BudgetMax { get; set; }

        public string Status { get; set; } = string.Empty;
        public string Address { get; internal set; }
        public int RequiredHours { get; internal set; }
        public decimal BudgetMin { get; internal set; }
        public string RequestPriority { get; internal set; }
        public string RequestStatus { get; internal set; }
        public DateTime CreatedAt { get; internal set; }
    }
}