namespace NursingCarePlatform.Web.Models
{
    public class Availability
    {
        public int Id { get; set; }

        public int NurseId { get; set; }

        public string Day { get; set; } = string.Empty;

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public Nurse Nurse { get; set; } = null!;
    }
}