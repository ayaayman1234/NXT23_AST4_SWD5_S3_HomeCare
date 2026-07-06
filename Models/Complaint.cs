using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.Models
{
    public class Complaint
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public int? NurseId { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        public string Status { get; set; } = "Open";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Patient Patient { get; set; } = null!;

        public Nurse? Nurse { get; set; }
    }
}