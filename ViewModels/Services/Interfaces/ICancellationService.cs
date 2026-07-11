using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.ViewModels.Cancellation;

namespace NursingCarePlatform.Web.Services.Interfaces
{
    public interface ICancellationService
    {
        Task<ServiceResult> CreateCancellationAsync(
            string userId,
            CreateCancellationViewModel model);

        Task<List<CancellationHistoryViewModel>> GetAllAsync();
    }
}