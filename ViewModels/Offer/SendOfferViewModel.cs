using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.ViewModels.Offer
{
    public class SendOfferViewModel
    {
        public int CareRequestId { get; set; }

        [Required]
        [Range(1, 100000)]
        public decimal ProposedPrice { get; set; }

        [Required]
        [StringLength(500)]
        public string Message { get; set; } = string.Empty;
    }
}