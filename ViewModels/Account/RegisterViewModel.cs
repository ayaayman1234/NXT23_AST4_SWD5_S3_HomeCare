using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string Governorate { get; set; } = string.Empty;

        [Required]
        public string Gender { get; set; } = string.Empty;

        [Range(18, 100)]
        public int Age { get; set; }

        [Required]
        public string Role { get; set; } = string.Empty;

        // Patient

        public string? MedicalHistory { get; set; }

        // Nurse

        public int YearsExperience { get; set; }

        public string? Specialization { get; set; }
    }
}