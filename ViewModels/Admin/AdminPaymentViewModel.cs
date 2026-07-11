namespace NursingCarePlatform.Web.ViewModels.Admin
{
    public class AdminPaymentViewModel
    {
        public int Id { get; set; }

        public int CareRequestId { get; set; }

        public decimal Amount { get; set; }

        public decimal CommissionAmount { get; set; }

        public decimal NetAmount { get; set; }

        public string PaymentMethod { get; set; } = "";

        public string PaymentStatus { get; set; } = "";

        public string TransactionReference { get; set; } = "";

        public DateTime PaymentDate { get; set; }
    }
}