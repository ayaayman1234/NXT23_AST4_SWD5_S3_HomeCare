namespace NursingCarePlatform.Web.Models
{
    public class NurseDocument
    {
        public int Id { get; set; }

        public int NurseId { get; set; }

        public string DocumentType { get; set; } = string.Empty;

        public string FilePath { get; set; } = string.Empty;

        public DateTime UploadDate { get; set; }

        public Nurse Nurse { get; set; } = null!;
        public bool IsApproved { get; set; }
    }
}