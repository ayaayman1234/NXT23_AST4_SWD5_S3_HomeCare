namespace NursingCarePlatform.Web.ViewModels.SOS
{
    public class SOSHistoryViewModel
    {
        public int Id { get; set; }

        public string TriggeredBy { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public string SOSStatus { get; set; } = string.Empty;
    }
}