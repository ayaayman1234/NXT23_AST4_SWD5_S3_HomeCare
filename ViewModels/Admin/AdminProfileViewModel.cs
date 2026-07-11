using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.ViewModels.Admin
{
    public class AdminProfileViewModel
    {
        [Required]
        public string FirstName { get; set; } = "";

        [Required]
        public string LastName { get; set; } = "";

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Phone]
        public string? PhoneNumber { get; set; }

        public string? ProfilePhoto { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}