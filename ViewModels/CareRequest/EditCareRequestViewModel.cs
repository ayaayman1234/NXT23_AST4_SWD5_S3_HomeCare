using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.ViewModels
{
    public class EditCareRequestViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Address { get; set; } = string.Empty;

        public int RequiredHours { get; set; }

        public bool OvernightStay { get; set; }

        public bool AccommodationAvailable { get; set; }

        public decimal BudgetMin { get; set; }

        public decimal BudgetMax { get; set; }

        public string? PreferredNurseGender { get; set; }

        public string RequestPriority { get; set; } = string.Empty;

        public string MatchingType { get; set; } = string.Empty;

        //Checklist

        public string BloodType { get; set; } = string.Empty;

        public string Allergies { get; set; } = string.Empty;

        public string ChronicDiseases { get; set; } = string.Empty;

        public bool ContagiousStatus { get; set; }
    }
}