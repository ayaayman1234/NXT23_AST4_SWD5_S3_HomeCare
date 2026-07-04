namespace NursingCarePlatform.Web.ViewModels
{
    public class CareRequestDetailsViewModel
    {
        public int Id { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public int RequiredHours { get; set; }

        public bool OvernightStay { get; set; }

        public bool AccommodationAvailable { get; set; }

        public decimal BudgetMin { get; set; }

        public decimal BudgetMax { get; set; }

        public string? PreferredNurseGender { get; set; }

        public string RequestPriority { get; set; } = string.Empty;

        public string RequestStatus { get; set; } = string.Empty;

        public string MatchingType { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        // Checklist

        public string BloodType { get; set; } = string.Empty;

        public string Allergies { get; set; } = string.Empty;

        public string ChronicDiseases { get; set; } = string.Empty;

        public bool ContagiousStatus { get; set; }
    }
}