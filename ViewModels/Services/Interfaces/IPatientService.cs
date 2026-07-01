using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.ViewModels.Patient;

namespace NursingCarePlatform.Web.Services.Interfaces
{
    public interface IPatientService
    {
        Task<ServiceResult> UpdateProfileAsync(
            string userId,
            EditPatientViewModel model);

        Task<Patient?> GetPatientByUserIdAsync(string userId);

        Task<List<CareRequest>> GetMyRequestsAsync(string userId);

        Task<ServiceResult> DeleteRequestAsync(int requestId);

        Task<ServiceResult> CancelRequestAsync(int requestId);
    }
}