using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.ViewModels.Cancellation
{
    public class CreateCancellationViewModel
    {
        public int CareRequestId { get; set; }

        [Required]
        public string Reason { get; set; } = "";
    }
}