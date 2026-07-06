using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.ViewModels.Complaint
{
    public class ComplaintViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nurse")]
        public int? NurseId { get; set; }

        [Required]
        [Display(Name = "Complaint Title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;
    }
}