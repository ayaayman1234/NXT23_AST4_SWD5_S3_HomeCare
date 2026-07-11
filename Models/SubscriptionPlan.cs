using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.Models
{
    /// <summary>
    /// Represents a nurse subscription plan that offers reduced or zero commission
    /// in exchange for a fixed periodic fee.
    /// Requirement: "The system may also support subscription plans for nurses,
    /// where a fixed fee is paid periodically in exchange for reduced or no commission."
    /// </summary>
    public class SubscriptionPlan
    {
        public int Id { get; set; }

        // ==========================
        // Plan Info
        // ==========================

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        // ==========================
        // Financials
        // ==========================

        /// <summary>Monthly subscription fee in EGP.</summary>
        [Required]
        [Range(0, 100000)]
        public decimal MonthlyFee { get; set; }

        /// <summary>
        /// Commission rate applied when this plan is active (0.00 – 1.00).
        /// 0 = no commission, 0.05 = 5%, etc.
        /// </summary>
        [Required]
        [Range(0.0, 1.0)]
        public decimal CommissionRate { get; set; }

        // ==========================
        // Status
        // ==========================

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // ==========================
        // Navigation
        // ==========================

        public ICollection<NurseSubscription> NurseSubscriptions { get; set; }
            = new List<NurseSubscription>();
    }
}
