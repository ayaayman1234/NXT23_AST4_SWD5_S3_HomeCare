using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.ViewModels.Assignment
{
    public class AssignmentViewModel
    {
        public int Id { get; set; }

        [Required]
        public int CareRequestId { get; set; }

        [Required]
        public int NurseId { get; set; }

        [Display(Name = "Shift Start")]
        public DateTime ShiftStart { get; set; }

        [Display(Name = "Shift End")]
        public DateTime ShiftEnd { get; set; }
        public DateTime? CompletedAt { get; set; }

        public string AssignmentStatus { get; set; } = string.Empty;
    }
}