namespace NursingCarePlatform.Web.ViewModels.Admin
{
    public class AdminReportViewModel
    {
        // ==========================
        // Users
        // ==========================

        public int TotalPatients { get; set; }

        public int TotalNurses { get; set; }

        public int TotalAdmins { get; set; }

        // ==========================
        // Care Requests
        // ==========================

        public int TotalCareRequests { get; set; }

        public int PendingRequests { get; set; }

        public int AcceptedRequests { get; set; }

        public int CompletedRequests { get; set; }

        // ==========================
        // Finance
        // ==========================

        public decimal TotalRevenue { get; set; }

        public decimal TotalCommission { get; set; }

        public int TotalPayments { get; set; }

        // ==========================
        // Ratings
        // ==========================

        public int TotalRatings { get; set; }

        public double AverageRating { get; set; }

        // ==========================
        // Issues
        // ==========================

        public int TotalComplaints { get; set; }

        public int OpenComplaints { get; set; }

        public int ResolvedComplaints { get; set; }

        public int TotalSOS { get; set; }

        public int PendingSOS { get; set; }

        public int TotalCancellations { get; set; }
    }
}