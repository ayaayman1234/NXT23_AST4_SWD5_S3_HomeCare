namespace NursingCarePlatform.Web.Models
{
    public class CareRequest
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

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

        public Patient Patient { get; set; } = null!;
    }
}

