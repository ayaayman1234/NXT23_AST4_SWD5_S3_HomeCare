namespace NursingCarePlatform.Web.Models
{
    public class MedicalChecklist
    {
        public int Id { get; set; }

        public int CareRequestId { get; set; }

        public string BloodType { get; set; } = string.Empty;

        public string Allergies { get; set; } = string.Empty;

        public string ChronicDiseases { get; set; } = string.Empty;

        public bool ContagiousStatus { get; set; }

        public CareRequest CareRequest { get; set; } = null!;
    }
}