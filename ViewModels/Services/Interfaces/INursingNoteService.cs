using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.ViewModels.NursingNote;

namespace NursingCarePlatform.Web.Services.Interfaces
{
    public interface INursingNoteService
    {
        Task<CreateNursingNoteViewModel?> GetAssignmentAsync(int assignmentId);

        Task<ServiceResult> CreateAsync(CreateNursingNoteViewModel model);

        Task<bool> HasNoteAsync(int assignmentId);
    }
}