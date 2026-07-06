using System.ComponentModel.DataAnnotations;

namespace NursingCarePlatform.Web.ViewModels.Patient
{
    public class EditPatientViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        // =========================
        // User Information
        // =========================

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Gender")]
        public string? Gender { get; set; }

        [Range(1, 120)]
        public int Age { get; set; }

        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Display(Name = "City")]
        public string? City { get; set; }

        [Display(Name = "Governorate")]
        public string? Governorate { get; set; }

        // =========================
        // Patient Information
        // =========================
        [Display(Name = "Blood Type")]
        public string? BloodType { get; set; }

        [Display(Name = "Medical History")]
        [StringLength(1000)]
        public string? MedicalHistory { get; set; }
    }
}