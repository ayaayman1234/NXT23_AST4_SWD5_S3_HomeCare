namespace NursingCarePlatform.Web.ViewModels.Complaint
{
    public class ComplaintHistoryViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Against { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}