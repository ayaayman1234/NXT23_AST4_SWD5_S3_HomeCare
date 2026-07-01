namespace NursingCarePlatform.Web.ViewModels.Nurse
{
    public class NurseProfileViewModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public int YearsExperience { get; set; }

        public string Specialization { get; set; } = string.Empty;

        public bool IsVerified { get; set; }
    }
}