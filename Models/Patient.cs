namespace NursingCarePlatform.Web.Models
{
    public class Patient
    {
        public int Id { get; set; }

        // Identity User Id
        public string UserId { get; set; } = string.Empty;

        public string MedicalHistory { get; set; } = string.Empty;

        public ApplicationUser User { get; set; } = null!;
    }
}