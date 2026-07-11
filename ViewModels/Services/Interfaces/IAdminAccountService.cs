using Microsoft.AspNetCore.Identity;
using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.ViewModels.Admin;

namespace NursingCarePlatform.Web.Services.Interfaces
{
    public interface IAdminAccountService
    {
        Task<ServiceResult> LoginAsync(AdminLoginViewModel model);

        Task LogoutAsync();

        Task<ServiceResult> ChangePasswordAsync(
            string userId,
            ChangePasswordViewModel model);

        Task<AdminProfileViewModel?> GetProfileAsync(string userId);
    }
}