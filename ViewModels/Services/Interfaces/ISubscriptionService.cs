using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.ViewModels.Admin;

namespace NursingCarePlatform.Web.Services.Interfaces
{
    /// <summary>
    /// Service for managing nurse subscription plans.
    /// Requirement: "The system may also support subscription plans for nurses,
    /// where a fixed fee is paid periodically in exchange for reduced or no commission.
    /// All financial operations must be recorded clearly."
    /// </summary>
    public interface ISubscriptionService
    {
        // ==========================
        // Plans
        // ==========================

        Task<List<AdminSubscriptionPlanViewModel>> GetAllPlansAsync();

        Task<AdminSubscriptionPlanViewModel?> GetPlanByIdAsync(int planId);

        Task<ServiceResult> CreatePlanAsync(CreateSubscriptionPlanViewModel model);

        Task<ServiceResult> UpdatePlanAsync(int planId, CreateSubscriptionPlanViewModel model);

        Task<ServiceResult> TogglePlanStatusAsync(int planId);

        Task<ServiceResult> DeletePlanAsync(int planId);

        // ==========================
        // Nurse Subscriptions
        // ==========================

        Task<List<AdminNurseSubscriptionViewModel>> GetAllNurseSubscriptionsAsync();

        Task<List<AdminNurseSubscriptionViewModel>> GetNurseSubscriptionsAsync(int nurseId);

        Task<ServiceResult> AssignPlanToNurseAsync(int nurseId, int planId, DateTime startDate, DateTime endDate);

        Task<ServiceResult> CancelSubscriptionAsync(int subscriptionId);

        /// <summary>
        /// Gets the active subscription commission rate for a nurse.
        /// Returns the plan's commission rate if the nurse has an active plan,
        /// otherwise returns the default commission rate.
        /// </summary>
        Task<decimal> GetEffectiveCommissionRateAsync(int nurseId, decimal defaultRate);
    }
}
