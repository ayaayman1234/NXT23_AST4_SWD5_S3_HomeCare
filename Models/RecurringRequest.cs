namespace NursingCarePlatform.Web.Models
{
    public class RecurringRequest
    {
        public int Id { get; set; }

        public int CareRequestId { get; set; }

        public int RepetitionCount { get; set; }

        public int FrequencyInterval { get; set; }

        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date of the recurring period (optional if Duration is specified).
        /// Requirement: "start date, and end date or duration"
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Duration in days as an alternative to EndDate.
        /// Requirement: "start date, and end date or duration"
        /// </summary>
        public int? Duration { get; set; }

        public CareRequest CareRequest { get; set; } = null!;
    }
}