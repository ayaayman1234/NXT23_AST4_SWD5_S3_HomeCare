using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.ViewModels.Complaint
{
    public class CreateComplaintViewModel
    {
        [Required]
        public int? NurseId { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;
    }
}