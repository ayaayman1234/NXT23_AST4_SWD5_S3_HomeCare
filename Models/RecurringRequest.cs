namespace NursingCarePlatform.Web.Models
{
    public class RecurringRequest
    {
        public int Id { get; set; }

        public int CareRequestId { get; set; }

        public int RepetitionCount { get; set; }

        public int FrequencyInterval { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public CareRequest CareRequest { get; set; } = null!;
    }
}