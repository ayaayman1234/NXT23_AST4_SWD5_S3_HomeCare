using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.ViewModels.MedicalChecklist;

namespace NursingCarePlatform.Web.Services.Interfaces
{
    public interface IMedicalChecklistService
    {
        Task<CreateMedicalChecklistViewModel?> GetRequestAsync(int requestId);

        Task<ServiceResult> CreateAsync(CreateMedicalChecklistViewModel model);
        Task<MedicalChecklist?> GetByRequestIdAsync(int requestId);

        Task<bool> ExistsAsync(int requestId);
    }
}