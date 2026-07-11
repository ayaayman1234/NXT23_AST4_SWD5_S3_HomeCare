namespace NursingCarePlatform.Web.ViewModels.Admin
{
    public class AdminCareRequestViewModel
    {
        public int Id { get; set; }

        public string PatientName { get; set; } = "";

        public string NurseName { get; set; } = "";

        public string ServiceName { get; set; } = "";

        public string Address { get; set; } = "";

        public string Description { get; set; } = "";

        public DateTime PreferredDate { get; set; }

        public int RequiredHours { get; set; }

        public decimal BudgetMin { get; set; }

        public decimal BudgetMax { get; set; }

        public string RequestPriority { get; set; } = "";

        public string Status { get; set; } = "";
    }
}