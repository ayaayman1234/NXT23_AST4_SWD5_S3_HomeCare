using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.ViewModels.Rating;

namespace NursingCarePlatform.Web.Services.Interfaces
{
    public interface IRatingService
    {
        // ==========================================
        // Add Rating
        // ==========================================

        Task<ServiceResult> AddRatingAsync(
            string userId,
            CreateRatingViewModel model);

        // ==========================================
        // Nurse Ratings
        // ==========================================

        Task<List<Rating>> GetNurseRatingsAsync(int nurseId);

        Task<double> GetAverageRatingAsync(int nurseId);

        Task<int> GetRatingsCountAsync(int nurseId);
    }
}