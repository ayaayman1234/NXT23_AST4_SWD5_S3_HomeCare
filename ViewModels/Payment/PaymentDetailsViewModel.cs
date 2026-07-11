namespace NursingCarePlatform.Web.ViewModels.Payment
{
    public class PaymentDetailsViewModel
    {
        public int Id { get; set; }

        public int CareRequestId { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public string NurseName { get; set; } = string.Empty;

        public string ServiceName { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public decimal CommissionAmount { get; set; }

        public decimal NetAmount { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;

        public string TransactionReference { get; set; } = string.Empty;

        public string PaymentStatus { get; set; } = string.Empty;

        public DateTime PaymentDate { get; set; }
    }
}