namespace NursingCarePlatform.Web.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        public int TotalPatients { get; set; }

        public int TotalNurses { get; set; }

        public int TotalRequests { get; set; }

        public int TotalAssignments { get; set; }

        public int TotalPayments { get; set; }

        public decimal TotalRevenue { get; set; }
    }
}