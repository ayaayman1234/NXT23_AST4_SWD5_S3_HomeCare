using System;

namespace NursingCarePlatform.Web.ViewModels
{
    public class AssignmentDetailsViewModel
    {
        public int Id { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string NurseName { get; set; } = string.Empty;
        public DateTime ShiftStart { get; set; }
        public DateTime ShiftEnd { get; set; }
        public string AssignmentStatus { get; set; } = string.Empty;
        public int CareRequestId { get; set; }
    }
}
