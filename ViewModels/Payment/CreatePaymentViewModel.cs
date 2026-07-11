using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.ViewModels.Payment
{
    public class CreatePaymentViewModel
    {
        [Required]
        public int CareRequestId { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public string NurseName { get; set; } = string.Empty;

        public string ServiceName { get; set; } = string.Empty;

        [Required]
        [Range(1, 1000000)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Please select payment method")]
        public string PaymentMethod { get; set; } = string.Empty;

        public string TransactionReference { get; set; } = string.Empty;
    }
}