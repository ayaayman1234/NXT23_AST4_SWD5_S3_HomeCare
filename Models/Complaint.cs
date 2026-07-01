namespace NursingCarePlatform.Web.Models
{
    public class Complaint
    {
        public int Id { get; set; }

        public int CreatedByUserId { get; set; }

        public int AgainstUserId { get; set; }

        public int? CareRequestId { get; set; }

        public string Reason { get; set; } = string.Empty;

        public string ComplaintStatus { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}