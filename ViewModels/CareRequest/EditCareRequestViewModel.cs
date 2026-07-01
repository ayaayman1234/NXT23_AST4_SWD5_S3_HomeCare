using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.ViewModels.CareRequest
{
    public class EditCareRequestViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        public DateTime PreferredDate { get; set; }

        public decimal BudgetMin { get; set; }

        public decimal BudgetMax { get; set; }
    }
}