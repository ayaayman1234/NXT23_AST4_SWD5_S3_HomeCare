namespace NursingCarePlatform.Web.ViewModels.Payment
{
    public class PaymentDetailsViewModel
    {
        public int Id { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;

        public string PaymentStatus { get; set; } = string.Empty;

        public DateTime PaymentDate { get; set; }
    }
}