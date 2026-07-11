using Microsoft.EntityFrameworkCore;
using NursingCarePlatform.Web.Data;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels.Cancellation;

namespace NursingCarePlatform.Web.Services.Implementations
{
    public class CancellationService : ICancellationService
    {
        private readonly NursingDbContext _context;
        private readonly INotificationService _notificationService;

        public CancellationService(
            NursingDbContext context,
            INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        // ==========================================
        // Create Cancellation
        // ==========================================

        public async Task<ServiceResult> CreateCancellationAsync(
            string userId,
            CreateCancellationViewModel model)
        {
            var request = await _context.CareRequests
                .Include(r => r.Patient)
                .Include(r => r.Nurse)
                .FirstOrDefaultAsync(r => r.Id == model.CareRequestId);

            if (request == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Care Request not found."
                };
            }

            int requestedById;
            string requestedByType;

            var patient = await _context.Patients
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (patient != null)
            {
                requestedById = patient.Id;
                requestedByType = "Patient";
            }
            else
            {
                var nurse = await _context.Nurses
                    .FirstOrDefaultAsync(x => x.UserId == userId);

                if (nurse == null)
                {
                    return new ServiceResult
                    {
                        Success = false,
                        Message = "User not found."
                    };
                }

                requestedById = nurse.Id;
                requestedByType = "Nurse";
            }

            var cancellation = new Cancellation
            {
                CareRequestId = request.Id,
                RequestedById = requestedById,
                RequestedByType = requestedByType,
                Reason = model.Reason,
                Fee = 0,
                RequestedAt = DateTime.Now,
                Status = "Pending"
            };

            _context.Cancellations.Add(cancellation);

            request.RequestStatus = "Cancelled";

            await _context.SaveChangesAsync();

            // ==========================================
            // Notifications
            // ==========================================

            if (requestedByType == "Patient")
            {
                if (request.NurseId != null)
                {
                    await _notificationService.CreateAsync(
                        request.NurseId.Value,
                        "Nurse",
                        "Cancellation Request",
                        "The patient has cancelled the care request.",
                        "Cancellation");
                }
            }
            else
            {
                await _notificationService.CreateAsync(
                    request.PatientId,
                    "Patient",
                    "Cancellation Request",
                    "The nurse has cancelled the care request.",
                    "Cancellation");
            }

            return new ServiceResult
            {
                Success = true,
                Message = "Cancellation request submitted successfully."
            };
        }

        // ==========================================
        // History
        // ==========================================

        public async Task<List<CancellationHistoryViewModel>> GetAllAsync()
        {
            return await _context.Cancellations
                .OrderByDescending(x => x.RequestedAt)
                .Select(x => new CancellationHistoryViewModel
                {
                    Id = x.Id,
                    CareRequestId = x.CareRequestId,
                    RequestedBy = x.RequestedByType,
                    Reason = x.Reason,
                    Fee = x.Fee,
                    Status = x.Status,
                    RequestedAt = x.RequestedAt
                })
                .ToListAsync();
        }
    }
}