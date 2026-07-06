using System.ComponentModel.DataAnnotations.Schema;

namespace NursingCarePlatform.Web.Models
{
    public class WorkHistory
    {
        public int Id { get; set; }

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

        public DateTime CompletedAt { get; set; }

        public int RequiredHours { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
    }
}