using Microsoft.EntityFrameworkCore;
using NursingCarePlatform.Web.Data;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels.Nurse;
using NurseServiceModel = NursingCarePlatform.Web.Models.NurseService;

namespace NursingCarePlatform.Web.Services.Implementations
{
    public class NurseManagementService : INurseService
    {
        private readonly NursingDbContext _context;

        public NurseManagementService(NursingDbContext context)
        {
            _context = context;
        }

        public async Task<Nurse?> GetNurseByUserIdAsync(string userId)
        {
            return await _context.Nurses
                .Include(n => n.User)
                .Include(n => n.Documents)
                .Include(n => n.Availabilities)
                .Include(n => n.NurseServices)
                .FirstOrDefaultAsync(n => n.UserId == userId);
        }

        public async Task<ServiceResult> UpdateProfileAsync(
            string userId,
            EditNurseViewModel model)
        {
            var nurse = await _context.Nurses
                .FirstOrDefaultAsync(n => n.UserId == userId);

            if (nurse == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Nurse not found."
                };
            }

            nurse.YearsExperience = model.YearsExperience;
            nurse.Specialization = model.Specialization;
            nurse.IsVerified = model.IsVerified;

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Profile updated successfully."
            };
        }

        public async Task<List<Availability>> GetAvailabilityAsync(string userId)
        {
            var nurse = await _context.Nurses
                .FirstOrDefaultAsync(n => n.UserId == userId);

            if (nurse == null)
                return new List<Availability>();

            return await _context.Availabilities
                .Where(a => a.NurseId == nurse.Id)
                .OrderBy(a => a.Day)
                .ToListAsync();
        }

        public async Task<List<NurseDocument>> GetDocumentsAsync(string userId)
        {
            var nurse = await _context.Nurses
                .FirstOrDefaultAsync(n => n.UserId == userId);

            if (nurse == null)
                return new List<NurseDocument>();

            return await _context.NurseDocuments
                .Where(d => d.NurseId == nurse.Id)
                .OrderByDescending(d => d.UploadDate)
                .ToListAsync();
        }

        public async Task<List<NurseServiceModel>> GetServicesAsync(string userId)
        {
            var nurse = await _context.Nurses
                .FirstOrDefaultAsync(n => n.UserId == userId);

            if (nurse == null)
                return new List<NurseServiceModel>();

            return await _context.NurseServices
                .Include(ns => ns.Service)
                .Where(ns => ns.NurseId == nurse.Id)
                .ToListAsync();
        }
    }
}