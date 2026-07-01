using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.ViewModels.Patient
{
    public class EditPatientViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Display(Name = "Medical History")]
        [StringLength(1000)]
        public string? MedicalHistory { get; set; }
    }
}