using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.ViewModels.WorkHistory;

namespace NursingCarePlatform.Web.Services.Interfaces
{
    public interface IAssignmentService
    {
        Task<List<Assignment>> GetNurseAssignmentsAsync(string userId);

        Task<Assignment?> GetAssignmentAsync(int id);

        Task<ServiceResult> CompleteAssignmentAsync(int assignmentId);
        Task<List<WorkHistoryViewModel>> GetWorkHistoryAsync(string userId);
    }
}