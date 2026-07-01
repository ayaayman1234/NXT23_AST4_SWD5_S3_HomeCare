namespace NursingCarePlatform.Web.Models
{
    public class NursingNote
    {
        public int Id { get; set; }

        public int AssignmentId { get; set; }

        public string BloodPressure { get; set; } = string.Empty;

        public decimal GlucoseLevel { get; set; }

        public decimal Temperature { get; set; }

        public string? Notes { get; set; }

        public DateTime UploadedAt { get; set; }

        public Assignment Assignment { get; set; } = null!;
    }
}