namespace NursingCarePlatform.Web.Models
{
    public class Patient
    {
        public int Id { get; set; }

        // Identity User Id
        public string UserId { get; set; } = string.Empty;

        // Medical Information
        public string? BloodType { get; set; }

        public string MedicalHistory { get; set; } = string.Empty;

        // Navigation Property
        public ApplicationUser User { get; set; } = null!;

        // Display Properties
        public string FirstName { get; internal set; } = string.Empty;

        public string LastName { get; internal set; } = string.Empty;
        public bool IsBlocked { get; set; } = false;
    }
}