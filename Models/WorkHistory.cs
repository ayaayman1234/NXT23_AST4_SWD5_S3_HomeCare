namespace NursingCarePlatform.Web.Models
{

    public class WorkHistory
    {
        public int Id { get; set; }

        public int AssignmentId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string JobStatus { get; set; } = string.Empty;

        public Assignment Assignment { get; set; } = null!;
    }
}