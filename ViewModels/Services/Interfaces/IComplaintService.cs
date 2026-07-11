using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.ViewModels.Complaint;

namespace NursingCarePlatform.Web.Services.Interfaces
{
    public interface IComplaintService
    {
        Task<ServiceResult> CreateComplaintAsync(
            string userId,
            CreateComplaintViewModel model);

        Task<List<ComplaintHistoryViewModel>> GetPatientComplaintsAsync(
            string userId);

        Task<List<ComplaintHistoryViewModel>> GetAllComplaintsAsync();

        Task<ComplaintDetailsViewModel?> GetComplaintDetailsAsync(int id);

        Task<ServiceResult> ResolveComplaintAsync(int id);
    }
}