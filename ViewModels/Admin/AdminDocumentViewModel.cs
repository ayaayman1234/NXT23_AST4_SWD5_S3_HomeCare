namespace NursingCarePlatform.Web.ViewModels.Admin
{
    public class AdminDocumentViewModel
    {
        public int Id { get; set; }

        public int NurseId { get; set; }

        public string NurseName { get; set; } = "";

        public string DocumentType { get; set; } = "";

        public string FilePath { get; set; } = "";

        public DateTime UploadedAt { get; set; }

        public bool IsApproved { get; set; }

        // ==========================
        // GAP 7 – Expiry tracking
        // Requirement: "The system ensures that nurse criminal record documents
        // are renewed periodically."
        // ==========================

        /// <summary>Expiry date from the Verification record (nullable).</summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>True if this document has an expiry date that has passed.</summary>
        public bool IsExpired => ExpiryDate.HasValue && ExpiryDate.Value < DateTime.Now;

        /// <summary>True if expiry is within the next 30 days (warning zone).</summary>
        public bool ExpiringsSoon => ExpiryDate.HasValue
            && ExpiryDate.Value >= DateTime.Now
            && ExpiryDate.Value <= DateTime.Now.AddDays(30);

        public string VerificationStatus { get; set; } = "";
    }
}