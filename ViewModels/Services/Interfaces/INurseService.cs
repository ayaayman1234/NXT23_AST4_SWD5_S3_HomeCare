using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.ViewModels.Nurse;

namespace NursingCarePlatform.Web.Services.Interfaces
{
    public interface INurseService
    {
        // ==========================
        // Profile
        // ==========================

        Task<Nurse?> GetNurseByUserIdAsync(string userId);

        Task<NurseProfileViewModel?> GetProfileAsync(string userId);

        Task<ServiceResult> UpdateProfileAsync(
            string userId,
            EditNurseViewModel model);

        // ==========================
        // Availability
        // ==========================

        Task<List<Availability>> GetAvailabilityAsync(string userId);

        Task<ServiceResult> AddAvailabilityAsync(
            string userId,
            AvailabilityViewModel model);

        Task<ServiceResult> DeleteAvailabilityAsync(int id);

        // ==========================
        // Documents
        // ==========================

        Task<List<NurseDocument>> GetDocumentsAsync(string userId);

        Task<ServiceResult> UploadDocumentAsync(
            string userId,
            DocumentViewModel model);

        Task<ServiceResult> DeleteDocumentAsync(int id);

        // ==========================
        // Service Categories (NEW)
        // ==========================

        Task<List<ServiceCategory>> GetCategoriesAsync();

        Task<List<NursingService>> GetServicesByCategoryAsync(int categoryId);

        // ==========================
        // Services
        // ==========================

        Task<List<NursingService>> GetAllServicesAsync();

        Task<List<NurseService>> GetServicesAsync(string userId);

        Task<ServiceResult> AddServiceAsync(
            string userId,
            NurseServiceViewModel model);

        Task<ServiceResult> UpdateServiceAsync(
            string userId,
            NurseServiceViewModel model);

        Task<ServiceResult> DeleteServiceAsync(
            string userId,
            int serviceId);
        // ==========================
        // Available Requests
        // ==========================

        Task<List<CareRequest>> GetAvailableRequestsAsync(string userId);
    }
}