namespace NursingCarePlatform.Web.ViewModels.WorkHistory
{
    public class WorkHistoryViewModel
    {
        public int AssignmentId { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public string ServiceName { get; set; } = string.Empty;

        public DateTime ShiftStart { get; set; }

        public DateTime ShiftEnd { get; set; }

        public DateTime? CompletedAt { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}