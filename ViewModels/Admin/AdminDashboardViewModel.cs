namespace NursingCarePlatform.Web.ViewModels.Admin
{
    public class AdminDashboardViewModel
    {
        public int TotalPatients { get; set; }

        public int TotalNurses { get; set; }

        public int TotalAdmins { get; set; }

        public int TotalCareRequests { get; set; }

        public int PendingRequests { get; set; }

        public int AcceptedRequests { get; set; }

        public int CompletedRequests { get; set; }

        public int TotalComplaints { get; set; }

        public int OpenComplaints { get; set; }

        public int TotalSOS { get; set; }

        public int PendingSOS { get; set; }

        public int TotalCancellations { get; set; }

        public int TotalPayments { get; set; }

        public decimal TotalRevenue { get; set; }

        public double AverageRating { get; set; }
    }
}