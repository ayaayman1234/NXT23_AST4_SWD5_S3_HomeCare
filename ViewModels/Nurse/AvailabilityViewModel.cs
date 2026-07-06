using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.ViewModels.Nurse
{
    public class AvailabilityViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Day { get; set; } = string.Empty;

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }
    }
}