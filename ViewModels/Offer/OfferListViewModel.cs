namespace NursingCarePlatform.Web.ViewModels.Offer
{
    public class OfferListViewModel
    {
        public int Id { get; set; }

        public string NurseName { get; set; } = string.Empty;

        public decimal ProposedPrice { get; set; }

        public string OfferStatus { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}