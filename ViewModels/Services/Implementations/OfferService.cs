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
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (nurse == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Nurse not found."
                };
            }

            var request = await _context.CareRequests
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

            // Prevent duplicate offer

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

            // قبول العرض
            offer.OfferStatus = "Accepted";

           
            offer.CareRequest.RequestStatus = "Accepted";
            offer.CareRequest.NurseId = offer.NurseId;

            
            var otherOffers = await _context.Offers
                .Where(o =>
                    o.CareRequestId == offer.CareRequestId &&
                    o.Id != offer.Id)
                .ToListAsync();

            foreach (var item in otherOffers)
            {
                item.OfferStatus = "Rejected";
            }

           
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
            var nurse = await _context.Nurses
    .FirstOrDefaultAsync(n => n.Id == offer.NurseId);

            if (nurse != null)
            {
                await _notificationService.CreateAsync(
    nurse.UserId,
    "Account Approved",
    "Congratulations! Your nurse account has been approved by the administrator.",
    "AccountApproved"
);
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
        }
    }
}