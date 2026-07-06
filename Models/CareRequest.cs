using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.Models
{
    public class CareRequest
    {
        public int Id { get; set; }

        // ================= Patient =================

        public int PatientId { get; set; }

        public Patient Patient { get; set; } = null!;

        // ================= Nurse =================

        public int? NurseId { get; set; }

        public Nurse? Nurse { get; set; }

        // ================= Service =================

        public int ServiceId { get; set; }

        public NursingService Service { get; set; } = null!;

        // ================= Request Info =================

        [Required]
        [StringLength(500)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        public DateTime PreferredDate { get; set; }

        public int RequiredHours { get; set; }

        public bool OvernightStay { get; set; }

        public bool AccommodationAvailable { get; set; }

        public decimal BudgetMin { get; set; }

        public decimal BudgetMax { get; set; }

        public string? PreferredNurseGender { get; set; }

        public string RequestPriority { get; set; } = "Normal";

        public string RequestStatus { get; set; } = "Pending";

        public string MatchingType { get; set; } = "Automatic";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsRated { get; set; } = false;

        // ================= Offers =================

        public ICollection<MyOffer> Offers { get; set; } = new List<MyOffer>();
        public MedicalChecklist? MedicalChecklist { get; set; }
    }
}