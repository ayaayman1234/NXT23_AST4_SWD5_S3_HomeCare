using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.ViewModels.Patient
{
    public class CreatePatientViewModel
    {
        [Required]
        public int UserId { get; set; }

        [Display(Name = "Medical History")]
        [StringLength(1000)]
        public string? MedicalHistory { get; set; }
    }
}