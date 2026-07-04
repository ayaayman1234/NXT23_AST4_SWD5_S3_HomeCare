namespace NursingCarePlatform.Web.ViewModels
{
    public class CareRequestListViewModel
    {
        public int Id { get; set; }

        public string Address { get; set; } = string.Empty;

        public int RequiredHours { get; set; }

        public decimal BudgetMin { get; set; }

        public decimal BudgetMax { get; set; }

        public string RequestPriority { get; set; } = string.Empty;

        public string RequestStatus { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}