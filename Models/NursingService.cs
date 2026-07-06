using System.Collections.Generic;

namespace NursingCarePlatform.Web.Models
{
    public class NursingService
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; }

        public ServiceCategory Category { get; set; } = null!;

        // Nurses who provide this service
        public ICollection<NurseService> NurseServices { get; set; }
            = new List<NurseService>();

        // Care Requests requesting this service
        public ICollection<CareRequest> CareRequests { get; set; }
            = new List<CareRequest>();
    }
}