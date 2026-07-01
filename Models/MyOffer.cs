namespace NursingCarePlatform.Web.Models
{
    public class MyOffer
    {
        public int Id { get; set; }

        public int CareRequestId { get; set; }

        public int NurseId { get; set; }

        public decimal ProposedPrice { get; set; }

        public string Message { get; set; } = string.Empty;

        public string OfferStatus { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public CareRequest CareRequest { get; set; } = null!;

        public Nurse Nurse { get; set; } = null!;
    }
}