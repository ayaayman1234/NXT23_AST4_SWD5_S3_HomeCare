namespace NursingCarePlatform.Web.Models
{
    public class Verification
    {
        public int Id { get; set; }

        public int NurseDocumentId { get; set; }

        public int AdminId { get; set; }

        public string VerificationStatus { get; set; } = string.Empty;

        public DateTime? ExpiryDate { get; set; }

        public DateTime VerificationTime { get; set; }

        public string? Notes { get; set; }

        public NurseDocument NurseDocument { get; set; } = null!;

        public Admin Admin { get; set; } = null!;
    }
}