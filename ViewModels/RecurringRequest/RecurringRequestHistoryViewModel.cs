namespace NursingCarePlatform.Web.ViewModels.RecurringRequest
{
    public class RecurringRequestHistoryViewModel
    {
        public int Id { get; set; }

        public int CareRequestId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int RepetitionCount { get; set; }

        public int FrequencyInterval { get; set; }
    }
}