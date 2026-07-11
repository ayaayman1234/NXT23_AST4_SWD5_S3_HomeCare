using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.Models
{
    /// <summary>
    /// Tracks which subscription plan a nurse is subscribed to,
    /// including the subscription period and current status.
    /// Requirement: "The system may also support subscription plans for nurses,
    /// where a fixed fee is paid periodically in exchange for reduced or no commission.
    /// All financial operations must be recorded clearly."
    /// </summary>
    public class NurseSubscription
    {
        public int Id { get; set; }

        // ==========================
        // Relations
        // ==========================

        [Required]
        public int NurseId { get; set; }

        public Nurse Nurse { get; set; } = null!;

        [Required]
        public int PlanId { get; set; }

        public SubscriptionPlan Plan { get; set; } = null!;

        // ==========================
        // Subscription Period
        // ==========================

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        // ==========================
        // Status
        // ==========================

        /// <summary>Active | Expired | Cancelled</summary>
        [StringLength(30)]
        public string Status { get; set; } = "Active";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
