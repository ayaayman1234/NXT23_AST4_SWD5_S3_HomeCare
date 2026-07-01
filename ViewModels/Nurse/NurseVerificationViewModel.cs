using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.ViewModels.Nurse
{
    public class NurseVerificationViewModel
    {
        [Required]
        public int NurseId { get; set; }

        public bool IsVerified { get; set; }

        public string? Notes { get; set; }
    }
}