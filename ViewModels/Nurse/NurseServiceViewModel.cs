using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.ViewModels.Nurse
{
    public class NurseServiceViewModel
    {
        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        public string ServiceName { get; set; } = "";

        public string CategoryName { get; set; } = "";

        [Required]
        [Range(0, 100000)]
        public decimal Price { get; set; }
    }
}