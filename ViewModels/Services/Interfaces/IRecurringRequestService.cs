using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.ViewModels.RecurringRequest;

namespace NursingCarePlatform.Web.Services.Interfaces
{
    public interface IRecurringRequestService
    {
        Task<ServiceResult> CreateRecurringRequestAsync(CreateRecurringRequestViewModel model);

        Task<List<RecurringRequestHistoryViewModel>> GetAllRecurringRequestsAsync();

        Task<RecurringRequestDetailsViewModel?> GetRecurringRequestDetailsAsync(int id);
    }
}