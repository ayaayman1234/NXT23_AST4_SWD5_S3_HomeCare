namespace NursingCarePlatform.Web.Models
{

    public class Rating
    {
        public int Id { get; set; }

        public int CareRequestId { get; set; }

        public int RaterUserId { get; set; }

        public int RatedUserId { get; set; }

        public int RatingScore { get; set; }

        public string? RatingComment { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}