using Microsoft.EntityFrameworkCore;
using NursingCarePlatform.Web.Data;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels.Admin;

namespace NursingCarePlatform.Web.Services.Implementations
{
    /// <summary>
    /// Implements nurse subscription plan management.
    /// Requirement: "The system may also support subscription plans for nurses,
    /// where a fixed fee is paid periodically in exchange for reduced or no commission."
    /// </summary>
    public class SubscriptionService : ISubscriptionService
    {
        private readonly NursingDbContext _context;

        public SubscriptionService(NursingDbContext context)
        {
            _context = context;
        }

        // ==========================
        // Get All Plans
        // ==========================

        public async Task<List<AdminSubscriptionPlanViewModel>> GetAllPlansAsync()
        {
            return await _context.SubscriptionPlans
                .OrderBy(p => p.MonthlyFee)
                .Select(p => new AdminSubscriptionPlanViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    MonthlyFee = p.MonthlyFee,
                    CommissionRate = p.CommissionRate,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt,
                    ActiveSubscribersCount = p.NurseSubscriptions
                        .Count(ns => ns.Status == "Active" && ns.EndDate >= DateTime.Now)
                })
                .ToListAsync();
        }

        // ==========================
        // Get Plan By Id
        // ==========================

        public async Task<AdminSubscriptionPlanViewModel?> GetPlanByIdAsync(int planId)
        {
            return await _context.SubscriptionPlans
                .Where(p => p.Id == planId)
                .Select(p => new AdminSubscriptionPlanViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    MonthlyFee = p.MonthlyFee,
                    CommissionRate = p.CommissionRate,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt,
                    ActiveSubscribersCount = p.NurseSubscriptions
                        .Count(ns => ns.Status == "Active" && ns.EndDate >= DateTime.Now)
                })
                .FirstOrDefaultAsync();
        }

        // ==========================
        // Create Plan
        // ==========================

        public async Task<ServiceResult> CreatePlanAsync(CreateSubscriptionPlanViewModel model)
        {
            var plan = new SubscriptionPlan
            {
                Name = model.Name,
                Description = model.Description,
                MonthlyFee = model.MonthlyFee,
                CommissionRate = model.CommissionRate,
                IsActive = model.IsActive,
                CreatedAt = DateTime.Now
            };

            _context.SubscriptionPlans.Add(plan);
            await _context.SaveChangesAsync();

            return new ServiceResult { Success = true, Message = "Subscription plan created successfully." };
        }

        // ==========================
        // Update Plan
        // ==========================

        public async Task<ServiceResult> UpdatePlanAsync(int planId, CreateSubscriptionPlanViewModel model)
        {
            var plan = await _context.SubscriptionPlans.FindAsync(planId);

            if (plan == null)
                return new ServiceResult { Success = false, Message = "Plan not found." };

            plan.Name = model.Name;
            plan.Description = model.Description;
            plan.MonthlyFee = model.MonthlyFee;
            plan.CommissionRate = model.CommissionRate;
            plan.IsActive = model.IsActive;

            await _context.SaveChangesAsync();

            return new ServiceResult { Success = true, Message = "Plan updated successfully." };
        }

        // ==========================
        // Toggle Plan Status
        // ==========================

        public async Task<ServiceResult> TogglePlanStatusAsync(int planId)
        {
            var plan = await _context.SubscriptionPlans.FindAsync(planId);

            if (plan == null)
                return new ServiceResult { Success = false, Message = "Plan not found." };

            plan.IsActive = !plan.IsActive;
            await _context.SaveChangesAsync();

            var status = plan.IsActive ? "activated" : "deactivated";
            return new ServiceResult { Success = true, Message = $"Plan {status} successfully." };
        }

        // ==========================
        // Delete Plan
        // ==========================

        public async Task<ServiceResult> DeletePlanAsync(int planId)
        {
            var hasActiveSubscriptions = await _context.NurseSubscriptions
                .AnyAsync(ns => ns.PlanId == planId && ns.Status == "Active");

            if (hasActiveSubscriptions)
                return new ServiceResult
                {
                    Success = false,
                    Message = "Cannot delete a plan with active subscribers. Deactivate it instead."
                };

            var plan = await _context.SubscriptionPlans.FindAsync(planId);

            if (plan == null)
                return new ServiceResult { Success = false, Message = "Plan not found." };

            _context.SubscriptionPlans.Remove(plan);
            await _context.SaveChangesAsync();

            return new ServiceResult { Success = true, Message = "Plan deleted successfully." };
        }

        // ==========================
        // Get All Nurse Subscriptions
        // ==========================

        public async Task<List<AdminNurseSubscriptionViewModel>> GetAllNurseSubscriptionsAsync()
        {
            return await _context.NurseSubscriptions
                .Include(ns => ns.Nurse).ThenInclude(n => n.User)
                .Include(ns => ns.Plan)
                .OrderByDescending(ns => ns.StartDate)
                .Select(ns => new AdminNurseSubscriptionViewModel
                {
                    Id = ns.Id,
                    NurseId = ns.NurseId,
                    NurseName = ns.Nurse.User.FirstName + " " + ns.Nurse.User.LastName,
                    PlanName = ns.Plan.Name,
                    MonthlyFee = ns.Plan.MonthlyFee,
                    CommissionRate = ns.Plan.CommissionRate,
                    StartDate = ns.StartDate,
                    EndDate = ns.EndDate,
                    Status = ns.Status
                })
                .ToListAsync();
        }

        // ==========================
        // Get Nurse Subscriptions By Nurse
        // ==========================

        public async Task<List<AdminNurseSubscriptionViewModel>> GetNurseSubscriptionsAsync(int nurseId)
        {
            return await _context.NurseSubscriptions
                .Include(ns => ns.Nurse).ThenInclude(n => n.User)
                .Include(ns => ns.Plan)
                .Where(ns => ns.NurseId == nurseId)
                .OrderByDescending(ns => ns.StartDate)
                .Select(ns => new AdminNurseSubscriptionViewModel
                {
                    Id = ns.Id,
                    NurseId = ns.NurseId,
                    NurseName = ns.Nurse.User.FirstName + " " + ns.Nurse.User.LastName,
                    PlanName = ns.Plan.Name,
                    MonthlyFee = ns.Plan.MonthlyFee,
                    CommissionRate = ns.Plan.CommissionRate,
                    StartDate = ns.StartDate,
                    EndDate = ns.EndDate,
                    Status = ns.Status
                })
                .ToListAsync();
        }

        // ==========================
        // Assign Plan to Nurse
        // ==========================

        public async Task<ServiceResult> AssignPlanToNurseAsync(int nurseId, int planId, DateTime startDate, DateTime endDate)
        {
            var nurse = await _context.Nurses.FindAsync(nurseId);
            if (nurse == null)
                return new ServiceResult { Success = false, Message = "Nurse not found." };

            var plan = await _context.SubscriptionPlans.FindAsync(planId);
            if (plan == null || !plan.IsActive)
                return new ServiceResult { Success = false, Message = "Plan not found or not active." };

            // Cancel any existing active subscription
            var existing = await _context.NurseSubscriptions
                .Where(ns => ns.NurseId == nurseId && ns.Status == "Active")
                .ToListAsync();

            foreach (var sub in existing)
                sub.Status = "Cancelled";

            // Create new subscription
            _context.NurseSubscriptions.Add(new NurseSubscription
            {
                NurseId = nurseId,
                PlanId = planId,
                StartDate = startDate,
                EndDate = endDate,
                Status = "Active",
                CreatedAt = DateTime.Now
            });

            await _context.SaveChangesAsync();

            return new ServiceResult { Success = true, Message = "Subscription assigned successfully." };
        }

        // ==========================
        // Cancel Subscription
        // ==========================

        public async Task<ServiceResult> CancelSubscriptionAsync(int subscriptionId)
        {
            var sub = await _context.NurseSubscriptions.FindAsync(subscriptionId);

            if (sub == null)
                return new ServiceResult { Success = false, Message = "Subscription not found." };

            sub.Status = "Cancelled";
            await _context.SaveChangesAsync();

            return new ServiceResult { Success = true, Message = "Subscription cancelled successfully." };
        }

        // ==========================
        // Get Effective Commission Rate
        // ==========================

        public async Task<decimal> GetEffectiveCommissionRateAsync(int nurseId, decimal defaultRate)
        {
            var activeSub = await _context.NurseSubscriptions
                .Include(ns => ns.Plan)
                .Where(ns => ns.NurseId == nurseId
                          && ns.Status == "Active"
                          && ns.EndDate >= DateTime.Now)
                .OrderByDescending(ns => ns.StartDate)
                .FirstOrDefaultAsync();

            return activeSub?.Plan.CommissionRate ?? defaultRate;
        }
    }
}
