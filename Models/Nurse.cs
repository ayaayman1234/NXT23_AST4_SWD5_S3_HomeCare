namespace NursingCarePlatform.Web.Models
{
    public class Nurse
    {
        public int Id { get; set; }

        // Identity User Id
        public string UserId { get; set; } = string.Empty;

        public int YearsExperience { get; set; }

        public string Specialization { get; set; } = string.Empty;

        public bool IsVerified { get; set; }
        public bool IsAvailable { get; set; } = true;
        public bool IsBlocked { get; set; } = false;

        public ApplicationUser User { get; set; } = null!;

        public ICollection<NurseDocument> Documents { get; set; }
            = new List<NurseDocument>();

        public ICollection<NurseService> NurseServices { get; set; }
            = new List<NurseService>();

        public ICollection<Availability> Availabilities { get; set; }
            = new List<Availability>();
    }
}