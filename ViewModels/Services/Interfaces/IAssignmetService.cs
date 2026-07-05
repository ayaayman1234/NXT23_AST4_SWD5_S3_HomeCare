using NursingCarePlatform.Web.ViewModels;

namespace NursingCarePlatform.Web.Services.Interfaces
{
    public interface IAssignmentService
    {
        Task<int> CreateAssignmentAsync(AssignmentViewModel model);
        Task<AssignmentDetailsViewModel?> GetAssignmentDetailsAsync(int id);
    }
}