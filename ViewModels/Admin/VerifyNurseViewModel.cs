using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.ViewModels.Admin
{
    public class VerifyNurseViewModel
    {
        public int NurseId { get; set; }

        [Display(Name = "Nurse Name")]
        public string NurseName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        [Display(Name = "Years of Experience")]
        public int YearsExperience { get; set; }

        public string Specialization { get; set; } = string.Empty;

        public bool IsVerified { get; set; }
    }
}