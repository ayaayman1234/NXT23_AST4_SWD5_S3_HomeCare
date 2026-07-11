using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.Models
{
    public class Payment
    {
        public int Id { get; set; }

        // ==========================
        // Relations
        // ==========================

        [Required]
        public int CareRequestId { get; set; }

        public CareRequest CareRequest { get; set; } = null!;

        // ==========================
        // Payment Information
        // ==========================

        [Required]
        [Range(1, 1000000)]
        public decimal Amount { get; set; }

        public decimal CommissionAmount { get; set; }

        public decimal NetAmount { get; set; }

        [Required]
        [StringLength(30)]
        public string PaymentMethod { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string PaymentStatus { get; set; } = "Completed";

        [StringLength(50)]
        public string TransactionReference { get; set; } = string.Empty;

        public DateTime PaymentDate { get; set; } = DateTime.Now;
    }
}