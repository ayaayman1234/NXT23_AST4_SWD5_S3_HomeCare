namespace NursingCarePlatform.Web.ViewModels.Complaint
{
    public class ComplaintDetailsViewModel
    {
        public int Id { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public string NurseName { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        
        public string? AdminNotes { get; set; }
    }
}