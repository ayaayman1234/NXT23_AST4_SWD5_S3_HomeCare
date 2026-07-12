using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NursingCarePlatform.Web.ViewModels.CareRequest
{
    public class CreateCareRequestViewModel
    {
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Please select a service.")]
        [Display(Name = "Service")]
        public int ServiceId { get; set; }

        [Required]
        [StringLength(500)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime PreferredDate { get; set; } = DateTime.Now.AddDays(1);

        [Range(1, 24)]
        public int RequiredHours { get; set; }

        public bool OvernightStay { get; set; }

        public bool AccommodationAvailable { get; set; }

        [Range(0, 100000)]
        public decimal BudgetMin { get; set; }

        [Range(0, 100000)]
        public decimal BudgetMax { get; set; }

        public string? PreferredNurseGender { get; set; }

        public string RequestPriority { get; set; } = "Normal";

        public string MatchingType { get; set; } = "Automatic";

        public IEnumerable<SelectListItem> Services { get; set; }
            = new List<SelectListItem>();

        public string BloodType { get; set; } = string.Empty;

        public string Allergies { get; set; } = string.Empty;

        public string ChronicDiseases { get; set; } = string.Empty;

        public bool ContagiousStatus { get; set; }
    }
}