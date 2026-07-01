using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.ViewModels.Rating
{
    public class RatingViewModel
    {
        public int Id { get; set; }

        [Required]
        public int AssignmentId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Stars { get; set; }

        [StringLength(500)]
        public string? Comment { get; set; }
    }
}