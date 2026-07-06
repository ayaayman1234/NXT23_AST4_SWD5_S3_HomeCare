using Microsoft.EntityFrameworkCore;
using NursingCarePlatform.Web.Data;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels.MedicalChecklist;

namespace NursingCarePlatform.Web.Services.Implementations
{
    public class MedicalChecklistService : IMedicalChecklistService
    {
        private readonly NursingDbContext _context;

        public MedicalChecklistService(NursingDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // Get Care Request
        // ==========================================

        public async Task<CreateMedicalChecklistViewModel?> GetRequestAsync(int requestId)
        {
            var request = await _context.CareRequests
                .Include(x => x.Patient)
                    .ThenInclude(p => p.User)
                .Include(x => x.Service)
                .FirstOrDefaultAsync(x => x.Id == requestId);

            if (request == null)
                return null;

            return new CreateMedicalChecklistViewModel
            {
                CareRequestId = request.Id,

                PatientName = request.Patient.User.FirstName + " " +
                              request.Patient.User.LastName,

                ServiceName = request.Service.Name,

                BloodType = string.Empty,
                Allergies = string.Empty,
                ChronicDiseases = string.Empty,
                ContagiousStatus = false
            };
        }
        public async Task<MedicalChecklist?> GetByRequestIdAsync(int requestId)
        {
            return await _context.MedicalChecklists
                .Include(x => x.CareRequest)
                .FirstOrDefaultAsync(x => x.CareRequestId == requestId);
        }

        // ==========================================
        // Check if checklist exists
        // ==========================================

        public async Task<bool> ExistsAsync(int requestId)
        {
            return await _context.MedicalChecklists
                .AnyAsync(x => x.CareRequestId == requestId);
        }

        // ==========================================
        // Create Checklist
        // ==========================================

        public async Task<ServiceResult> CreateAsync(CreateMedicalChecklistViewModel model)
        {
            if (await ExistsAsync(model.CareRequestId))
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Medical Checklist already exists."
                };
            }

            var checklist = new MedicalChecklist
            {
                CareRequestId = model.CareRequestId,

                BloodType = model.BloodType,

                Allergies = model.Allergies,

                ChronicDiseases = model.ChronicDiseases,

                ContagiousStatus = model.ContagiousStatus
            };

            _context.MedicalChecklists.Add(checklist);

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Medical Checklist saved successfully."
            };
        }
    }
}