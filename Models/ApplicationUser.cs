using Microsoft.AspNetCore.Identity;

namespace NursingCarePlatform.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string Governorate { get; set; } = string.Empty;

        public string ProfilePhoto { get; set; } = string.Empty;

        public string AccountStatus { get; set; } = "Active";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string Gender { get; set; } = string.Empty;

        public int Age { get; set; }
    }
}