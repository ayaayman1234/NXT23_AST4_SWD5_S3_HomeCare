namespace NursingCarePlatform.Web.ViewModels.Admin
{
    public class AdminSOSViewModel
    {
        public int Id { get; set; }

        public string PatientName { get; set; } = "";

        public string Location { get; set; } = "";

        public DateTime CreatedAt { get; set; }

        public string Status { get; set; } = "";
    }
}