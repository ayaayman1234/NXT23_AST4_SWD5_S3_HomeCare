namespace NursingCarePlatform.Web.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public int CareRequestId { get; set; }

        public decimal Amount { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;

        public string TransactionReference { get; set; } = string.Empty;

        public DateTime PaymentDate { get; set; }

        public string PaymentStatus { get; set; } = string.Empty;

        public CareRequest CareRequest { get; set; } = null!;
    }
}