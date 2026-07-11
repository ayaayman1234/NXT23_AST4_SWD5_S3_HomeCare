using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.ViewModels.SOS;

namespace NursingCarePlatform.Web.Services.Interfaces
{
    public interface ISOSService
    {
        Task<ServiceResult> CreateSOSAsync(
            string userId,
            CreateSOSViewModel model);

        Task<List<SOSHistoryViewModel>> GetAllSOSAsync();

        Task<SOSDetailsViewModel?> GetSOSDetailsAsync(int id);
    }
}