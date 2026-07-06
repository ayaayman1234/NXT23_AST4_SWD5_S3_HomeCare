using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.ViewModels.Admin;

namespace NursingCarePlatform.Web.Services.Interfaces
{
    public interface IAdminService
    {
        // =====================================
        // Dashboard
        // =====================================

        Task<int> GetTotalPatientsAsync();

        Task<int> GetTotalNursesAsync();

        Task<int> GetPendingNursesCountAsync();

        // =====================================
        // Pending Nurses
        // =====================================

        Task<List<Nurse>> GetPendingNursesAsync();

        Task<Nurse?> GetNurseByIdAsync(int nurseId);

        Task<ServiceResult> ApproveNurseAsync(int nurseId);

        Task<ServiceResult> RejectNurseAsync(int nurseId);

        // =====================================
        // Users
        // =====================================

        Task<List<UserViewModel>> GetAllUsersAsync();

        Task<ServiceResult> BlockUserAsync(string userId);

        Task<ServiceResult> UnblockUserAsync(string userId);
    }
}