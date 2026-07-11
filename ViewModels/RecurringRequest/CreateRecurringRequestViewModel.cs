using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.ViewModels.RecurringRequest
{
    public class CreateRecurringRequestViewModel
    {
        [Required]
        public int CareRequestId { get; set; }

        [Required]
        [Range(1, 365)]
        public int RepetitionCount { get; set; }

        [Required]
        [Range(1, 365)]
        public int FrequencyInterval { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}