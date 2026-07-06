using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.Models
{
    public class ServiceCategory
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public ICollection<NursingService> Services { get; set; }
    = new List<NursingService>();
    }
}