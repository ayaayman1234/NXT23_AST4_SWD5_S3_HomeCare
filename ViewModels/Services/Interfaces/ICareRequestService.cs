using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.ViewModels;
using NursingCarePlatform.Web.ViewModels.CareRequest;

namespace NursingCarePlatform.Web.Services
{
    public interface ICareRequestService
    {
        Task<List<CareRequestListViewModel>> GetAllRequestsAsync();

        Task<List<CareRequestListViewModel>> GetPatientRequestsAsync(int patientId);

        Task<CareRequestDetailsViewModel?> GetRequestDetailsAsync(int id);

        Task CreateRequestAsync(CreateCareRequestViewModel model, int patientId);

        Task EditRequestAsync(EditCareRequestViewModel model);

        Task DeleteRequestAsync(int id);

        Task<EditCareRequestViewModel?> GetRequestForEditAsync(int id);
    }
}