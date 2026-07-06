namespace NursingCarePlatform.Web.ViewModels.Nurse
{
    public class NurseProfileViewModel
    {
        public NursingCarePlatform.Web.Models.Nurse Nurse { get; set; } = null!;

        public double AverageRating { get; set; }

        public int ReviewsCount { get; set; }

        public List<NursingCarePlatform.Web.Models.Rating> Reviews { get; set; }
            = new();
    }
}