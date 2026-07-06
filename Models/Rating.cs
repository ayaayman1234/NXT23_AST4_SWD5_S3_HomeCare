using System.ComponentModel.DataAnnotations.Schema;

namespace NursingCarePlatform.Web.Models
{
    public class Rating
    {
        public int Id { get; set; }

        // ==========================
        // Care Request
        // ==========================

        public int CareRequestId { get; set; }

        public CareRequest CareRequest { get; set; } = null!;

        // ==========================
        // Users
        // ==========================

        public int RaterUserId { get; set; }

        public int RatedUserId { get; set; }

        // ==========================
        // Rating
        // ==========================

        public int RatingScore { get; set; }

        public string? RatingComment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}