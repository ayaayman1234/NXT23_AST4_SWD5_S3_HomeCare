namespace NursingCarePlatform.Web.Models
{
    public class NursingService
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public ICollection<NurseService> NurseServices
        {
            get;
            set;
        } = new List<NurseService>();
    }
}