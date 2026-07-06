using Microsoft.EntityFrameworkCore;
using NursingCarePlatform.Web.Data;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels.Rating;

namespace NursingCarePlatform.Web.Services.Implementations
{
    public class RatingService : IRatingService
    {
        private readonly NursingDbContext _context;

        public RatingService(NursingDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult> AddRatingAsync(string userId, CreateRatingViewModel model)
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

            
            var alreadyRated = await _context.Ratings.AnyAsync(r =>
                r.CareRequestId == model.CareRequestId &&
                r.RaterUserId == patient.Id);

            if (alreadyRated)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "You have already rated this request."
                };
            }

            var rating = new Rating
            {
                CareRequestId = model.CareRequestId,
                RaterUserId = patient.Id,
                RatedUserId = model.NurseId,
                RatingScore = model.Stars,
                RatingComment = model.Comment,
                CreatedAt = DateTime.Now
            };

            _context.Ratings.Add(rating);

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Rating submitted successfully."
            };
        }

        public async Task<List<Rating>> GetNurseRatingsAsync(int nurseId)
        {
            return await _context.Ratings
                .Where(r => r.RatedUserId == nurseId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<double> GetAverageRatingAsync(int nurseId)
        {
            var ratings = await _context.Ratings
                .Where(r => r.RatedUserId == nurseId)
                .ToListAsync();

            if (!ratings.Any())
                return 0;

            return ratings.Average(r => r.RatingScore);
        }

        public async Task<int> GetRatingsCountAsync(int nurseId)
        {
            return await _context.Ratings
                .CountAsync(r => r.RatedUserId == nurseId);
        }
    }
}