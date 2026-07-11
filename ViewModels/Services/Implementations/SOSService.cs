using Microsoft.EntityFrameworkCore;
using NursingCarePlatform.Web.Data;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels.SOS;

namespace NursingCarePlatform.Web.Services.Implementations
{
    public class SOSService : ISOSService
    {
        private readonly NursingDbContext _context;

        public SOSService(NursingDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult> CreateSOSAsync(
            string userId,
            CreateSOSViewModel model)
        {
            var patient = await _context.Patients
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (patient == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Patient not found."
                };
            }

            var sos = new SOSEvent
            {
                TriggeredByUserId = patient.Id,
                CareRequestId = model.CareRequestId,
                Location = model.Location,
                CreatedAt = DateTime.Now,
                SOSStatus = "Pending"
            };

            _context.SOSEvents.Add(sos);

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "SOS request sent successfully."
            };
        }

        public async Task<List<SOSHistoryViewModel>> GetAllSOSAsync()
        {
            return await _context.SOSEvents
                .Include(s => s.TriggeredByUser)
                    .ThenInclude(p => p.User)
                .Include(s => s.CareRequest)
                .OrderByDescending(s => s.CreatedAt)
                .Select(s => new SOSHistoryViewModel
                {
                    Id = s.Id,

                    TriggeredBy =
                        s.TriggeredByUser.User.FirstName + " " +
                        s.TriggeredByUser.User.LastName,

                    Location = s.Location,

                    CreatedAt = s.CreatedAt,

                    SOSStatus = s.SOSStatus
                })
                .ToListAsync();
        }

        public async Task<SOSDetailsViewModel?> GetSOSDetailsAsync(int id)
        {
            var sos = await _context.SOSEvents
                .Include(s => s.TriggeredByUser)
                    .ThenInclude(p => p.User)
                .Include(s => s.CareRequest)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sos == null)
                return null;

            return new SOSDetailsViewModel
            {
                Id = sos.Id,

                TriggeredBy =
                    sos.TriggeredByUser.User.FirstName + " " +
                    sos.TriggeredByUser.User.LastName,

                CareRequestId = sos.CareRequestId,

                Location = sos.Location,

                CreatedAt = sos.CreatedAt,

                SOSStatus = sos.SOSStatus
            };
        }
    }
}