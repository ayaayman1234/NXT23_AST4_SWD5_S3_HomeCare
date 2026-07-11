namespace NursingCarePlatform.Web.ViewModels.Admin
{
    public class AdminNurseViewModel
    {
        public int Id { get; set; }

        public string FullName { get; set; } = "";

        public string Email { get; set; } = "";
        public string ProfilePhoto { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Address { get; set; } = "";

        public string Governorate { get; set; } = "";

        public string City { get; set; } = "";

        public int Age { get; set; }
        public string Gender { get; set; } = "";


        public string Specialization { get; set; } = "";

        public int YearsExperience { get; set; }

        public bool IsVerified { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsAvailable { get; set; }
    }
}