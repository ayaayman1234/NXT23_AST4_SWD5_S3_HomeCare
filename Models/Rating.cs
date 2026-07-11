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
        // Users (int IDs from Patient/Nurse tables)
        // ==========================

        public int RaterUserId { get; set; }

        public int RatedUserId { get; set; }

        // ==========================
        // ApplicationUser links (string GUIDs)
        // GAP 4 – links rating to actual Identity users
        // Requirement: "rater reference, rated user reference"
        // ==========================

        /// <summary>ApplicationUser.Id (GUID string) of the person giving the rating.</summary>
        public string? RaterUserGuid { get; set; }

        [ForeignKey(nameof(RaterUserGuid))]
        public ApplicationUser? RaterUser { get; set; }

        /// <summary>ApplicationUser.Id (GUID string) of the person being rated.</summary>
        public string? RatedUserGuid { get; set; }

        [ForeignKey(nameof(RatedUserGuid))]
        public ApplicationUser? RatedUser { get; set; }

        // ==========================
        // Rating
        // ==========================

        public int RatingScore { get; set; }

        public string? RatingComment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}