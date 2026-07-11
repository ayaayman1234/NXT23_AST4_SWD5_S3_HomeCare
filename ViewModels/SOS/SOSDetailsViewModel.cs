namespace NursingCarePlatform.Web.ViewModels.SOS
{
    public class SOSDetailsViewModel
    {
        public int Id { get; set; }

        public string TriggeredBy { get; set; } = string.Empty;

        public int? CareRequestId { get; set; }

        public string Location { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public string SOSStatus { get; set; } = string.Empty;
    }
}