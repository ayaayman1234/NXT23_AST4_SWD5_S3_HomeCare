namespace NursingCarePlatform.Web.Models
{
    public class Admin
    {
        public int Id { get; set; }

        // Identity User Id
        public string UserId { get; set; } = string.Empty;

        public string Role { get; set; } = "Admin";

        public ApplicationUser User { get; set; } = null!;
    }
}