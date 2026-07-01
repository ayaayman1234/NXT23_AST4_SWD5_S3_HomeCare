namespace NursingCarePlatform.Web.ViewModels.CareRequest
{
    public class CareRequestDetailsViewModel
    {
        public int Id { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public string ServiceName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime PreferredDate { get; set; }

        public decimal BudgetMin { get; set; }

        public decimal BudgetMax { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}