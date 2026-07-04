using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.ViewModels
{
    public class CreateCareRequestViewModel
    {
        [Required]
        public string Address { get; set; } = string.Empty;

        [Range(1, 24)]
        public int RequiredHours { get; set; }

        public bool OvernightStay { get; set; }

        public bool AccommodationAvailable { get; set; }

        [Range(0, 100000)]
        public decimal BudgetMin { get; set; }

        [Range(0, 100000)]
        public decimal BudgetMax { get; set; }

        public string? PreferredNurseGender { get; set; }

        [Required]
        public string RequestPriority { get; set; } = string.Empty;

        [Required]
        public string MatchingType { get; set; } = string.Empty;

        // Medical Checklist

        [Required]
        public string BloodType { get; set; } = string.Empty;

        public string Allergies { get; set; } = string.Empty;

        public string ChronicDiseases { get; set; } = string.Empty;

        public bool ContagiousStatus { get; set; }
    }
}