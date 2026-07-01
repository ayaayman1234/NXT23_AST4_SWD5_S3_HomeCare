using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.ViewModels.Nurse
{
    public class CreateNurseViewModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [Range(0, 50)]
        [Display(Name = "Years Of Experience")]
        public int YearsExperience { get; set; }

        [Required]
        [StringLength(100)]
        public string Specialization { get; set; } = string.Empty;

        public bool IsVerified { get; set; }
    }
}