using Microsoft.EntityFrameworkCore;
using NursingCarePlatform.Web.Data;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels.RecurringRequest;

namespace NursingCarePlatform.Web.Services.Implementations
{
    public class RecurringRequestService : IRecurringRequestService
    {
        private readonly NursingDbContext _context;

        public RecurringRequestService(NursingDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult> CreateRecurringRequestAsync(CreateRecurringRequestViewModel model)
        {
            var request = await _context.CareRequests
                .FirstOrDefaultAsync(x => x.Id == model.CareRequestId);

            if (request == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Care Request not found."
                };
            }

            var recurring = new RecurringRequest
            {
                CareRequestId = model.CareRequestId,
                RepetitionCount = model.RepetitionCount,
                FrequencyInterval = model.FrequencyInterval,
                StartDate = model.StartDate,
                EndDate = model.EndDate
            };

            _context.RecurringRequests.Add(recurring);

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Recurring request created successfully."
            };
        }

        public async Task<List<RecurringRequestHistoryViewModel>> GetAllRecurringRequestsAsync()
        {
            return await _context.RecurringRequests
                .OrderByDescending(x => x.StartDate)
                .Select(x => new RecurringRequestHistoryViewModel
                {
                    Id = x.Id,
                    CareRequestId = x.CareRequestId,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    RepetitionCount = x.RepetitionCount,
                    FrequencyInterval = x.FrequencyInterval
                })
                .ToListAsync();
        }

        public async Task<RecurringRequestDetailsViewModel?> GetRecurringRequestDetailsAsync(int id)
        {
            return await _context.RecurringRequests
                .Include(r => r.CareRequest)
                    .ThenInclude(c => c.Patient)
                        .ThenInclude(p => p.User)
                .Include(r => r.CareRequest)
                    .ThenInclude(c => c.Service)
                .Where(r => r.Id == id)
                .Select(r => new RecurringRequestDetailsViewModel
                {
                    Id = r.Id,
                    CareRequestId = r.CareRequestId,
                    RepetitionCount = r.RepetitionCount,
                    FrequencyInterval = r.FrequencyInterval,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    ServiceName = r.CareRequest.Service.Name,
                    PatientName =
                        r.CareRequest.Patient.User.FirstName + " " +
                        r.CareRequest.Patient.User.LastName
                })
                .FirstOrDefaultAsync();
        }
    }
}