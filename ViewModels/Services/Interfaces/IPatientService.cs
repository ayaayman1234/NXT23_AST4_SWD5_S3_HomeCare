using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.ViewModels.CareRequest;
using NursingCarePlatform.Web.ViewModels.Patient;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace NursingCarePlatform.Web.Services.Interfaces
{
    public interface IPatientService
    {
        // ==========================
        // Profile
        // ==========================

        Task<Patient?> GetPatientByUserIdAsync(string userId);

        Task<ServiceResult> UpdateProfileAsync(
            string userId,
            EditPatientViewModel model);

        // ==========================
        // Care Requests
        // ==========================

        Task<ServiceResult> CreateRequestAsync(
            string userId,
            CreateCareRequestViewModel model);
        Task<List<SelectListItem>> GetServicesAsync();

        Task<List<CareRequest>> GetMyRequestsAsync(string userId);

        Task<CareRequest?> GetRequestByIdAsync(int requestId);

        Task<ServiceResult> UpdateRequestAsync(
            EditCareRequestViewModel model);

        Task<ServiceResult> DeleteRequestAsync(int requestId);

        Task<ServiceResult> CancelRequestAsync(int requestId);
    }
}