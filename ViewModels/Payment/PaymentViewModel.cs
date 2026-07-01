using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.ViewModels.Payment
{
    public class PaymentViewModel
    {
        public int Id { get; set; }

        [Required]
        public int AssignmentId { get; set; }

        [Required]
        [Range(1, 100000)]
        public decimal Amount { get; set; }

        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; } = string.Empty;

        public string PaymentStatus { get; set; } = string.Empty;
    }
}