using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.ViewModels.Nurse;

namespace NursingCarePlatform.Web.Services.Interfaces
{
    public interface INurseService
    {
        Task<Nurse?> GetNurseByUserIdAsync(string userId);

        Task<ServiceResult> UpdateProfileAsync(
            string userId,
            EditNurseViewModel model);

        Task<List<Availability>> GetAvailabilityAsync(string userId);

        Task<List<NurseDocument>> GetDocumentsAsync(string userId);

        Task<List<NursingCarePlatform.Web.Models.NurseService>> GetServicesAsync(string userId);
    }
}