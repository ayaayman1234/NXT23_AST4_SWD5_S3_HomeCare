using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.ViewModels.CareRequest
{
    public class CreateCareRequestViewModel
    {
        [Required]
        public int PatientId { get; set; }

        [Required]
        [Display(Name = "Service")]
        public int ServiceId { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Description")]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Preferred Date")]
        public DateTime PreferredDate { get; set; }

        [Display(Name = "Budget Min")]
        public decimal BudgetMin { get; set; }

        [Display(Name = "Budget Max")]
        public decimal BudgetMax { get; set; }
    }
}