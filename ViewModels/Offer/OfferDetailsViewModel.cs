namespace NursingCarePlatform.Web.ViewModels.Offer
{
    public class OfferDetailsViewModel
    {
        public int Id { get; set; }

        public int CareRequestId { get; set; }

        public string NurseName { get; set; } = string.Empty;

        public decimal ProposedPrice { get; set; }

        public string? Message { get; set; }

        public string OfferStatus { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}