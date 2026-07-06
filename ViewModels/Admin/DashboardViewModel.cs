namespace NursingCarePlatform.Web.ViewModels.Admin
{
    public class DashboardViewModel
    {
        public int TotalPatients { get; set; }

        public int TotalNurses { get; set; }

        public int PendingNurses { get; set; }

        public int TotalCareRequests { get; set; }

        public int TotalComplaints { get; set; }
    }
}