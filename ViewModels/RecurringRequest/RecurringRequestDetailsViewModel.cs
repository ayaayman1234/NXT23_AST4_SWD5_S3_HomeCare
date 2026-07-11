namespace NursingCarePlatform.Web.ViewModels.RecurringRequest
{
    public class RecurringRequestDetailsViewModel
    {
        public int Id { get; set; }

        public int CareRequestId { get; set; }

        public int RepetitionCount { get; set; }

        public int FrequencyInterval { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string ServiceName { get; set; } = string.Empty;

        public string PatientName { get; set; } = string.Empty;
    }
}