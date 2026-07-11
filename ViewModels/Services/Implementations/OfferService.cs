using Microsoft.EntityFrameworkCore;
using NursingCarePlatform.Web.Data;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels.Offer;

namespace NursingCarePlatform.Web.Services.Implementations
{
    public class OfferService : IOfferService
    {
        private readonly NursingDbContext _context;
        private readonly INotificationService _notificationService;
        public OfferService(NursingDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        // =====================================
        // Get Request
        // =====================================

        public async Task<SendOfferViewModel?> GetRequestAsync(int requestId)
        {
            var request = await _context.CareRequests
                .FirstOrDefaultAsync(x =>
                    x.Id == requestId &&
                    x.RequestStatus == "Pending");

            if (request == null)
                return null;

            return new SendOfferViewModel
            {
                CareRequestId = request.Id
            };
        }

        // =====================================
        // Send Offer
        // =====================================

        public async Task<ServiceResult> SendOfferAsync(
    string userId,
    SendOfferViewModel model)
        {
            var nurse = await _context.Nurses
                .Include(n => n.User)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (nurse == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Nurse not found."
                };
            }

            // Business Logic Rule: Nurse must be verified to send offers
            if (!nurse.IsVerified)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Your account is not verified yet. You must upload your documents and wait for admin approval before you can send offers."
                };
            }

            var request = await _context.CareRequests
                .Include(r => r.Patient)
                .FirstOrDefaultAsync(x =>
                    x.Id == model.CareRequestId &&
                    x.RequestStatus == "Pending");

            if (request == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Request not found."
                };
            }

            bool alreadySent = await _context.Offers.AnyAsync(x =>
                x.CareRequestId == model.CareRequestId &&
                x.NurseId == nurse.Id);

            if (alreadySent)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "You already sent an offer."
                };
            }

            var offer = new MyOffer
            {
                CareRequestId = model.CareRequestId,
                NurseId = nurse.Id,
                ProposedPrice = model.ProposedPrice,
                Message = model.Message,
                OfferStatus = "Pending",
                CreatedAt = DateTime.Now
            };

            _context.Offers.Add(offer);

            await _context.SaveChangesAsync();

            // ==========================
            // Notify Patient
            // ==========================

            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.Id == request.PatientId);

            if (patient != null)
            {
                await _notificationService.CreateAsync(
             patient.Id,
             "Patient",
             "New Offer",
             $"{nurse.User.FirstName} {nurse.User.LastName} has sent you an offer for your care request.",
             "Offer");
            }

            return new ServiceResult
            {
                Success = true,
                Message = "Offer sent successfully."
            };
        }
        public async Task<List<MyOffer>> GetOffersForRequestAsync(int requestId)
        {
            return await _context.Offers
                .Include(o => o.Nurse)
                    .ThenInclude(n => n.User)
                .Where(o =>
                    o.CareRequestId == requestId &&
                    o.OfferStatus == "Pending")
                .OrderBy(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task AcceptOfferAsync(int offerId)
        {
            var offer = await _context.Offers
                .Include(o => o.CareRequest)
                .FirstOrDefaultAsync(o => o.Id == offerId);

            if (offer == null)
                return;

            // ==========================
            // Accept selected offer
            // ==========================

            offer.OfferStatus = "Accepted";

            offer.CareRequest.RequestStatus = "Accepted";
            offer.CareRequest.NurseId = offer.NurseId;

            // ==========================
            // Reject other offers
            // ==========================

            var otherOffers = await _context.Offers
                .Where(o =>
                    o.CareRequestId == offer.CareRequestId &&
                    o.Id != offer.Id)
                .ToListAsync();

            foreach (var item in otherOffers)
            {
                item.OfferStatus = "Rejected";
            }

            // ==========================
            // Create Assignment
            // ==========================

            var assignment = new Assignment
            {
                CareRequestId = offer.CareRequestId,
                NurseId = offer.NurseId,
                ShiftStart = offer.CareRequest.PreferredDate,
                ShiftEnd = offer.CareRequest.PreferredDate
                    .AddHours(offer.CareRequest.RequiredHours),
                AssignmentStatus = "Assigned"
            };

            _context.Assignments.Add(assignment);

            await _context.SaveChangesAsync();

            // ==========================
            // Notify accepted nurse
            // ==========================

            var acceptedNurse = await _context.Nurses
                .FirstOrDefaultAsync(n => n.Id == offer.NurseId);

            if (acceptedNurse != null)
            {
                await _notificationService.CreateAsync(
    acceptedNurse.Id,
    "Nurse",
    "Offer Accepted",
    "Congratulations! Your offer has been accepted.",
    "OfferAccepted");
            }

            // ==========================
            // Notify rejected nurses
            // ==========================

            foreach (var rejectedOffer in otherOffers)
            {
                var rejectedNurse = await _context.Nurses
                    .FirstOrDefaultAsync(n => n.Id == rejectedOffer.NurseId);

                if (rejectedNurse != null)
                {
                    await _notificationService.CreateAsync(
    rejectedNurse.Id,
    "Nurse",
    "Offer Rejected",
    "Unfortunately, your offer was not selected.",
    "OfferRejected");
                }
            }
        }

        public async Task RejectOfferAsync(int offerId)
        {
            var offer = await _context.Offers
                .FirstOrDefaultAsync(x => x.Id == offerId);

            if (offer == null)
                return;

            offer.OfferStatus = "Rejected";

            await _context.SaveChangesAsync();

            // ==========================
            // Notify nurse
            // ==========================

            var nurse = await _context.Nurses
                .FirstOrDefaultAsync(n => n.Id == offer.NurseId);

            if (nurse != null)
            {
                await _notificationService.CreateAsync(
    nurse.Id,
    "Nurse",
    "Offer Rejected",
    "Your offer has been rejected by the patient.",
    "OfferRejected");
            }
        }
    }
}