using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NursingCarePlatform.Web.Data;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels.Admin;

namespace NursingCarePlatform.Web.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly NursingDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminService(
            NursingDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ==========================================
        // Dashboard
        // ==========================================

        public async Task<int> GetTotalPatientsAsync()
        {
            return await _context.Patients.CountAsync();
        }

        public async Task<int> GetTotalNursesAsync()
        {
            return await _context.Nurses.CountAsync();
        }

        public async Task<int> GetPendingNursesCountAsync()
        {
            return await _context.Nurses
                .CountAsync(n => !n.IsVerified);
        }

        // ==========================================
        // Pending Nurses
        // ==========================================

        public async Task<List<Nurse>> GetPendingNursesAsync()
        {
            return await _context.Nurses
                .Include(n => n.User)
                .Where(n => !n.IsVerified)
                .ToListAsync();
        }
        // ==========================================
        // Get Nurse By Id
        // ==========================================

        public async Task<Nurse?> GetNurseByIdAsync(int nurseId)
        {
            return await _context.Nurses
                .Include(n => n.User)
                .FirstOrDefaultAsync(n => n.Id == nurseId);
        }
        // ==========================================
        // Approve Nurse
        // ==========================================

        public async Task<ServiceResult> ApproveNurseAsync(int nurseId)
        {
            var nurse = await _context.Nurses
                .FirstOrDefaultAsync(n => n.Id == nurseId);

            if (nurse == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Nurse not found."
                };
            }

            nurse.IsVerified = true;

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Nurse approved successfully."
            };
        }

        // ==========================================
        // Reject Nurse
        // ==========================================

        public async Task<ServiceResult> RejectNurseAsync(int nurseId)
        {
            var nurse = await _context.Nurses
                .Include(n => n.User)
                .FirstOrDefaultAsync(n => n.Id == nurseId);

            if (nurse == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Nurse not found."
                };
            }

            _context.Nurses.Remove(nurse);

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Nurse rejected successfully."
            };
        }

        // ==========================================
        // Users
        // ==========================================

        public async Task<List<UserViewModel>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            var result = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                result.Add(new UserViewModel
                {
                    Id = user.Id,
                    FullName = $"{user.FirstName} {user.LastName}",
                    Email = user.Email ?? "",
                    PhoneNumber = user.PhoneNumber ?? "",
                    Role = roles.FirstOrDefault() ?? "",
                    AccountStatus = user.AccountStatus,
                    CreatedAt = user.CreatedAt
                });
            }

            return result;
        }

        public async Task<ServiceResult> BlockUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            user.AccountStatus = "Blocked";

            await _userManager.UpdateAsync(user);

            return new ServiceResult
            {
                Success = true,
                Message = "User blocked successfully."
            };
        }

        public async Task<ServiceResult> UnblockUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            user.AccountStatus = "Active";

            await _userManager.UpdateAsync(user);

            return new ServiceResult
            {
                Success = true,
                Message = "User unblocked successfully."
            };
        }
    }
}