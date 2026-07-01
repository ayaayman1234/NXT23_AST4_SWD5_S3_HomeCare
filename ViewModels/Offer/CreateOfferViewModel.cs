using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.ViewModels.Offer
{
    public class CreateOfferViewModel
    {
        [Required]
        public int CareRequestId { get; set; }

        [Required]
        public int NurseId { get; set; }

        [Required]
        [Range(1, 100000)]
        [Display(Name = "Proposed Price")]
        public decimal ProposedPrice { get; set; }

        [StringLength(1000)]
        public string? Message { get; set; }
    }
}