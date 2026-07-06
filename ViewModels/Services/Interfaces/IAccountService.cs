using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.ViewModels.Account;

namespace NursingCarePlatform.Web.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ServiceResult> RegisterAsync(RegisterViewModel model);

        Task<ServiceResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();

        Task<ApplicationUser?> GetUserByEmailAsync(string email);

        Task<bool> IsInRoleAsync(ApplicationUser user, string role);
    }
}