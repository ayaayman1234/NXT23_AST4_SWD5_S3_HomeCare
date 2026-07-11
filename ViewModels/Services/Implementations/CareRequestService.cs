using Microsoft.EntityFrameworkCore;
using NursingCarePlatform.Web.Data;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.ViewModels;

namespace NursingCarePlatform.Web.Services
{
    public class CareRequestService : ICareRequestService
    {
        private readonly NursingDbContext _context;

        public CareRequestService(NursingDbContext context)
        {
            _context = context;
        }

        public async Task<List<CareRequestListViewModel>> GetAllRequestsAsync()
        {
            return await _context.CareRequests
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new CareRequestListViewModel
                {
                    Id = x.Id,
                    Address = x.Address,
                    RequiredHours = x.RequiredHours,
                    BudgetMin = x.BudgetMin,
                    BudgetMax = x.BudgetMax,
                    RequestPriority = x.RequestPriority,
                    RequestStatus = x.RequestStatus,
                    CreatedAt = x.CreatedAt
                }).ToListAsync();
        }

        public async Task<List<CareRequestListViewModel>> GetPatientRequestsAsync(int patientId)
        {
            return await _context.CareRequests
                .Where(x => x.PatientId == patientId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new CareRequestListViewModel
                {
                    Id = x.Id,
                    Address = x.Address,
                    RequiredHours = x.RequiredHours,
                    BudgetMin = x.BudgetMin,
                    BudgetMax = x.BudgetMax,
                    RequestPriority = x.RequestPriority,
                    RequestStatus = x.RequestStatus,
                    CreatedAt = x.CreatedAt
                }).ToListAsync();
        }

        public async Task<CareRequestDetailsViewModel?> GetRequestDetailsAsync(int id)
        {
            var request = await _context.CareRequests
                .Include(x => x.Patient)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (request == null)
                return null;

            var checklist = await _context.MedicalChecklists
                .FirstOrDefaultAsync(x => x.CareRequestId == id);

            return new CareRequestDetailsViewModel
            {
                Id = request.Id,
                PatientName = request.Patient.FirstName + " " + request.Patient.LastName,
                Address = request.Address,
                RequiredHours = request.RequiredHours,
                OvernightStay = request.OvernightStay,
                AccommodationAvailable = request.AccommodationAvailable,
                BudgetMin = request.BudgetMin,
                BudgetMax = request.BudgetMax,
                PreferredNurseGender = request.PreferredNurseGender,
                RequestPriority = request.RequestPriority,
                RequestStatus = request.RequestStatus,
                MatchingType = request.MatchingType,
                CreatedAt = request.CreatedAt,

                BloodType = checklist?.BloodType ?? "",
                Allergies = checklist?.Allergies ?? "",
                ChronicDiseases = checklist?.ChronicDiseases ?? "",
                ContagiousStatus = checklist?.ContagiousStatus ?? false
            };
        }

        public async Task CreateRequestAsync(CreateCareRequestViewModel model, int patientId)
        {
            var request = new CareRequest
            {
                PatientId = patientId,
                Address = model.Address,
                RequiredHours = model.RequiredHours,
                OvernightStay = model.OvernightStay,
                AccommodationAvailable = model.AccommodationAvailable,
                BudgetMin = model.BudgetMin,
                BudgetMax = model.BudgetMax,
                PreferredNurseGender = model.PreferredNurseGender,
                RequestPriority = model.RequestPriority,
                RequestStatus = "Pending",
                MatchingType = model.MatchingType,
                CreatedAt = DateTime.Now
            };

            _context.CareRequests.Add(request);

            await _context.SaveChangesAsync();

            var checklist = new MedicalChecklist
            {
                CareRequestId = request.Id,
                BloodType = model.BloodType,
                Allergies = model.Allergies,
                ChronicDiseases = model.ChronicDiseases,
                ContagiousStatus = model.ContagiousStatus
            };

            _context.MedicalChecklists.Add(checklist);

            await _context.SaveChangesAsync();
        }

        public async Task<EditCareRequestViewModel?> GetRequestForEditAsync(int id)
        {
            var request = await _context.CareRequests
                .FirstOrDefaultAsync(x => x.Id == id);

            if (request == null)
                return null;

            var checklist = await _context.MedicalChecklists
                .FirstOrDefaultAsync(x => x.CareRequestId == id);

            return new EditCareRequestViewModel
            {
                Id = request.Id,
                Address = request.Address,
                RequiredHours = request.RequiredHours,
                OvernightStay = request.OvernightStay,
                AccommodationAvailable = request.AccommodationAvailable,
                BudgetMin = request.BudgetMin,
                BudgetMax = request.BudgetMax,
                PreferredNurseGender = request.PreferredNurseGender,
                RequestPriority = request.RequestPriority,
                MatchingType = request.MatchingType,

                BloodType = checklist?.BloodType ?? "",
                Allergies = checklist?.Allergies ?? "",
                ChronicDiseases = checklist?.ChronicDiseases ?? "",
                ContagiousStatus = checklist?.ContagiousStatus ?? false
            };
        }

        public async Task EditRequestAsync(EditCareRequestViewModel model)
        {
            var request = await _context.CareRequests.FindAsync(model.Id);

            if (request == null)
                return;

            request.Address = model.Address;
            request.RequiredHours = model.RequiredHours;
            request.OvernightStay = model.OvernightStay;
            request.AccommodationAvailable = model.AccommodationAvailable;
            request.BudgetMin = model.BudgetMin;
            request.BudgetMax = model.BudgetMax;
            request.PreferredNurseGender = model.PreferredNurseGender;
            request.RequestPriority = model.RequestPriority;
            request.MatchingType = model.MatchingType;

            var checklist = await _context.MedicalChecklists
                .FirstOrDefaultAsync(x => x.CareRequestId == model.Id);

            if (checklist != null)
            {
                checklist.BloodType = model.BloodType;
                checklist.Allergies = model.Allergies;
                checklist.ChronicDiseases = model.ChronicDiseases;
                checklist.ContagiousStatus = model.ContagiousStatus;
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteRequestAsync(int id)
        {
            var checklist = await _context.MedicalChecklists
                .FirstOrDefaultAsync(x => x.CareRequestId == id);

            if (checklist != null)
                _context.MedicalChecklists.Remove(checklist);

            var request = await _context.CareRequests.FindAsync(id);

            if (request != null)
                _context.CareRequests.Remove(request);

            await _context.SaveChangesAsync();
        }
    }
}