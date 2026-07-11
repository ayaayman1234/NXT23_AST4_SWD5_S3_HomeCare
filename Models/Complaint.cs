using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NursingCarePlatform.Web.Models
{
    public class Complaint
    {
        public int Id { get; set; }

        // ==========================
        // Existing References (Backward-compatible)
        // ==========================

        public int PatientId { get; set; }

        public int? NurseId { get; set; }

        // ==========================
        // GAP 5 – Generic User-to-User Complaint
        // Requirement: "Each complaint includes created-by user reference,
        // against-user reference, related care request reference (optional)"
        // This allows ANY user (nurse or patient) to file a complaint.
        // ==========================

        /// <summary>ApplicationUser.Id (GUID) of the user filing the complaint.</summary>
        public string? CreatedByUserId { get; set; }

        [ForeignKey(nameof(CreatedByUserId))]
        public ApplicationUser? CreatedByUser { get; set; }

        /// <summary>ApplicationUser.Id (GUID) of the user being complained about.</summary>
        public string? AgainstUserId { get; set; }

        [ForeignKey(nameof(AgainstUserId))]
        public ApplicationUser? AgainstUser { get; set; }

        /// <summary>
        /// Optional link to the care request this complaint is related to.
        /// Requirement: "related care request reference (optional)"
        /// </summary>
        public int? ComplaintCareRequestId { get; set; }

        [ForeignKey(nameof(ComplaintCareRequestId))]
        public CareRequest? ComplaintCareRequest { get; set; }

        // ==========================
        // Complaint Content
        // ==========================

        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        public string Status { get; set; } = "Open";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? AdminNotes { get; set; }

        // ==========================
        // Navigation
        // ==========================

        public Patient Patient { get; set; } = null!;

        public Nurse? Nurse { get; set; }
    }
}