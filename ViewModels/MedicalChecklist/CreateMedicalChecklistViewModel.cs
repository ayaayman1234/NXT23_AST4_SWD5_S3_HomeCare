using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.ViewModels.MedicalChecklist
{
    public class CreateMedicalChecklistViewModel
    {
        public int CareRequestId { get; set; }

        // ==========================
        // Display Information
        // ==========================

        public string PatientName { get; set; } = string.Empty;

        public string ServiceName { get; set; } = string.Empty;

        // ==========================
        // Medical Checklist
        // ==========================

        [Required]
        [Display(Name = "Blood Type")]
        public string BloodType { get; set; } = string.Empty;

        [Display(Name = "Allergies")]
        public string Allergies { get; set; } = string.Empty;

        [Display(Name = "Chronic Diseases")]
        public string ChronicDiseases { get; set; } = string.Empty;

        [Display(Name = "Contagious Case")]
        public bool ContagiousStatus { get; set; }
    }
}