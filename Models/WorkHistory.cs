using System.ComponentModel.DataAnnotations.Schema;

namespace NursingCarePlatform.Web.Models
{
    public class WorkHistory
    {
        public int Id { get; set; }

        // ==========================
        // Assignment (GAP 2 – required by spec)
        // ==========================

        /// <summary>
        /// Direct reference to the assignment that generated this work record.
        /// Requirement: "Each work history record includes a unique identifier,
        /// assignment reference, start time, end time, and job status."
        /// </summary>
        public int? AssignmentId { get; set; }

        public Assignment? Assignment { get; set; }

        // ==========================
        // Nurse
        // ==========================

        public int NurseId { get; set; }

        public Nurse Nurse { get; set; } = null!;

        // ==========================
        // Patient
        // ==========================

        public int PatientId { get; set; }

        public Patient Patient { get; set; } = null!;

        // ==========================
        // Care Request
        // ==========================

        public int CareRequestId { get; set; }

        public CareRequest CareRequest { get; set; } = null!;

        // ==========================
        // Service
        // ==========================

        public int ServiceId { get; set; }

        public NursingService Service { get; set; } = null!;

        // ==========================
        // Job Details
        // ==========================

        /// <summary>
        /// Job status for this work history record.
        /// Requirement: "Each work history record includes ... and job status."
        /// Values: Completed | Cancelled | InProgress
        /// </summary>
        public string JobStatus { get; set; } = "Completed";

        public DateTime StartTime { get; set; }

        public DateTime CompletedAt { get; set; }

        public int RequiredHours { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
    }
}