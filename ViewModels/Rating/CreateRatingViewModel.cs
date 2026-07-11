using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.ViewModels.Rating
{
    public class CreateRatingViewModel
    {
        public int CareRequestId { get; set; }

        public int NurseId { get; set; }

        public int RatedUserId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Stars { get; set; }

        [StringLength(500)]
        public string? Comment { get; set; }
    }
}