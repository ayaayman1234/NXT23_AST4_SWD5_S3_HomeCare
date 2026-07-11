namespace NursingCarePlatform.Web.ViewModels.Admin
{
    public class AdminRatingViewModel
    {
        public int Id { get; set; }

        public int CareRequestId { get; set; }

        public string PatientName { get; set; } = "";

        public string NurseName { get; set; } = "";

        public int RatingScore { get; set; }

        public string Comment { get; set; } = "";

        public DateTime CreatedAt { get; set; }

        public string Stars => new string('★', RatingScore) + new string('☆', 5 - RatingScore);
    }
}