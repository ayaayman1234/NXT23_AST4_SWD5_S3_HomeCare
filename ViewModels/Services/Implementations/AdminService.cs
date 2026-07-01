using Microsoft.EntityFrameworkCore;
using NursingCarePlatform.Web.Data;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.Services.Interfaces;

namespace NursingCarePlatform.Web.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly NursingDbContext _context;

        public AdminService(NursingDbContext context)
        {
            _context = context;
        }

        public async Task<List<Nurse>> GetPendingNursesAsync()
        {
            return await _context.Nurses
                .Include(n => n.User)
                .Where(n => !n.IsVerified)
                .ToListAsync();
        }

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

        public async Task<ServiceResult> RejectNurseAsync(int nurseId)
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

            _context.Nurses.Remove(nurse);

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Nurse rejected successfully."
            };
        }
    }
}