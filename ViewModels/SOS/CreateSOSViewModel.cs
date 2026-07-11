using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.ViewModels.SOS
{
    public class CreateSOSViewModel
    {
        public int? CareRequestId { get; set; }

        [Required]
        [StringLength(300)]
        public string Location { get; set; } = string.Empty;
    }
}