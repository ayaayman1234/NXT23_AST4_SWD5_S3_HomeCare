using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.ViewModels.Offer
{
    public class EditOfferViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public decimal ProposedPrice { get; set; }

        [StringLength(1000)]
        public string? Message { get; set; }
    }
}