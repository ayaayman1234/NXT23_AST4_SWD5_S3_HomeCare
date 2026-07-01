using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Models.Responses;

namespace NursingCarePlatform.Web.Services.Interfaces
{
    public interface IAdminService
    {
        Task<List<Nurse>> GetPendingNursesAsync();

        Task<ServiceResult> ApproveNurseAsync(int nurseId);

        Task<ServiceResult> RejectNurseAsync(int nurseId);
    }
}