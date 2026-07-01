using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.ViewModels.Complaint
{
    public class ComplaintViewModel
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        public string ComplaintStatus { get; set; } = string.Empty;
    }
}