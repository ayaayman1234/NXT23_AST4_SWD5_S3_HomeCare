namespace NursingCarePlatform.Web.Models
{

    public class Assignment
    {
        public int Id { get; set; }

        public int CareRequestId { get; set; }

        public int NurseId { get; set; }

        public DateTime ShiftStart { get; set; }

        public DateTime ShiftEnd { get; set; }

        public string AssignmentStatus { get; set; } = string.Empty;

        public CareRequest CareRequest { get; set; } = null!;

        public Nurse Nurse { get; set; } = null!;
    }
}