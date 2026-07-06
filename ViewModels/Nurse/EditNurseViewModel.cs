using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.ViewModels.Nurse
{
    public class EditNurseViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        // Identity

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? Governorate { get; set; }

        public string? Gender { get; set; }

        public int Age { get; set; }

        // Nurse

        [Display(Name = "Years of Experience")]
        public int YearsExperience { get; set; }

        [Display(Name = "Specialization")]
        public string Specialization { get; set; } = string.Empty;
    }
}