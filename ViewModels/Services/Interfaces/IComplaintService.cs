using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.ViewModels.Complaint;

namespace NursingCarePlatform.Web.Services.Interfaces
{
    public interface IComplaintService
    {
        Task<ServiceResult> CreateComplaintAsync(
            string userId,
            ComplaintViewModel model);

        Task<List<Complaint>> GetAllComplaintsAsync();

        Task<ServiceResult> ResolveComplaintAsync(int id);

        Task<ServiceResult> CloseComplaintAsync(int id);
    }
}