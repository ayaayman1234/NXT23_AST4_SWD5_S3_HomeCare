namespace NursingCarePlatform.Web.Models
{
    public class NurseService
    {
        public int NurseId { get; set; }

        public int ServiceId { get; set; }

        public decimal Price { get; set; }

        public Nurse Nurse { get; set; } = null!;

        public NursingService Service { get; set; } = null!;
    }
}