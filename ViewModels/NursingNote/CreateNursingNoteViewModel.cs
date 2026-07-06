using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.ViewModels.NursingNote
{
    public class CreateNursingNoteViewModel
    {
        public int AssignmentId { get; set; }

        [Required]
        [Display(Name = "Blood Pressure")]
        public string BloodPressure { get; set; } = string.Empty;

        [Required]
        [Range(20, 600)]
        [Display(Name = "Glucose Level")]
        public decimal GlucoseLevel { get; set; }

        [Required]
        [Range(30, 45)]
        [Display(Name = "Temperature")]
        public decimal Temperature { get; set; }

        [Display(Name = "Notes")]
        [StringLength(1000)]
        public string? Notes { get; set; }
    }
}