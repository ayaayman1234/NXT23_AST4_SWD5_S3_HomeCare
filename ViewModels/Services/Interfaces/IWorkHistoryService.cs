using NursingCarePlatform.Web.Models;

namespace NursingCarePlatform.Web.Services.Interfaces
{
    public interface IWorkHistoryService
    {
        Task<List<WorkHistory>> GetHistoryAsync(string userId);
    }
}