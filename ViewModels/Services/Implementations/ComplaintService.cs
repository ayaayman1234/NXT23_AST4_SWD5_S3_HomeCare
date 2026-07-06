using Microsoft.EntityFrameworkCore;
using NursingCarePlatform.Web.Data;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels.Complaint;

namespace NursingCarePlatform.Web.Services.Implementations
{
    public class ComplaintService : IComplaintService
    {
        private readonly NursingDbContext _context;

        public ComplaintService(NursingDbContext context)
        {
            _context = context;
        }

        // =====================================
        // Create Complaint
        // =====================================

        public async Task<ServiceResult> CreateComplaintAsync(
            string userId,
            ComplaintViewModel model)
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

            var complaint = new Complaint
            {
                PatientId = patient.Id,
                NurseId = model.NurseId,
                Title = model.Title,
                Description = model.Description,
                Status = "Open",
                CreatedAt = DateTime.Now
            };

            _context.Complaints.Add(complaint);

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Complaint submitted successfully."
            };
        }

        // =====================================
        // Get All Complaints
        // =====================================

        public async Task<List<Complaint>> GetAllComplaintsAsync()
        {
            return await _context.Complaints
                .Include(c => c.Patient)
                    .ThenInclude(p => p.User)
                .Include(c => c.Nurse)
                    .ThenInclude(n => n.User)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        // =====================================
        // Resolve Complaint
        // =====================================

        public async Task<ServiceResult> ResolveComplaintAsync(int id)
        {
            var complaint = await _context.Complaints
                .FirstOrDefaultAsync(x => x.Id == id);

            if (complaint == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Complaint not found."
                };
            }

            complaint.Status = "Resolved";

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Complaint resolved successfully."
            };
        }

        // =====================================
        // Close Complaint
        // =====================================

        public async Task<ServiceResult> CloseComplaintAsync(int id)
        {
            var complaint = await _context.Complaints
                .FirstOrDefaultAsync(x => x.Id == id);

            if (complaint == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Complaint not found."
                };
            }

            complaint.Status = "Closed";

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Complaint closed successfully."
            };
        }
    }
}