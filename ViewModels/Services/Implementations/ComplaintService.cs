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
        private readonly INotificationService _notificationService;

        public ComplaintService(
            NursingDbContext context,
            INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        // ==========================================
        // Create Complaint
        // ==========================================

        public async Task<ServiceResult> CreateComplaintAsync(
            string userId,
            CreateComplaintViewModel model)
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

            if (model.NurseId.HasValue)
            {
                var nurse = await _context.Nurses
                    .FirstOrDefaultAsync(x => x.Id == model.NurseId.Value);

                if (nurse == null)
                {
                    return new ServiceResult
                    {
                        Success = false,
                        Message = "Nurse not found."
                    };
                }
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

            // ==========================
            // Notify Admins
            // ==========================

            var admins = await _context.Admins.ToListAsync();

            foreach (var admin in admins)
            {
                await _notificationService.CreateAsync(
                    admin.Id,
                    "Admin",
                    "New Complaint",
                    "A new complaint has been submitted.",
                    "Complaint");
            }

            return new ServiceResult
            {
                Success = true,
                Message = "Complaint submitted successfully."
            };
        }

        // ==========================================
        // Patient History
        // ==========================================

        public async Task<List<ComplaintHistoryViewModel>> GetPatientComplaintsAsync(
    string userId)
        {
            var patient = await _context.Patients
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (patient == null)
                return new();

            return await _context.Complaints
                .Include(x => x.Nurse)
                    .ThenInclude(x => x.User)
                .Where(x => x.PatientId == patient.Id)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new ComplaintHistoryViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Against = x.Nurse == null
                        ? "-"
                        : x.Nurse.User.FirstName + " " +
                          x.Nurse.User.LastName,
                    Status = x.Status,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();
        }

        // ==========================================
        // Admin
        // ==========================================

        public async Task<List<ComplaintHistoryViewModel>> GetAllComplaintsAsync()
        {
            return await _context.Complaints
                .Include(x => x.Nurse)
                    .ThenInclude(x => x.User)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new ComplaintHistoryViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Against = x.Nurse == null
                        ? "-"
                        : x.Nurse.User.FirstName + " " +
                          x.Nurse.User.LastName,
                    Status = x.Status,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();
        }

        // ==========================================
        // Details
        // ==========================================

        public async Task<ComplaintDetailsViewModel?> GetComplaintDetailsAsync(int id)
        {
            var complaint = await _context.Complaints
                .Include(x => x.Patient)
                    .ThenInclude(x => x.User)
                .Include(x => x.Nurse)
                    .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (complaint == null)
                return null;

            return new ComplaintDetailsViewModel
            {
                Id = complaint.Id,
                PatientName =
                    complaint.Patient.User.FirstName + " " +
                    complaint.Patient.User.LastName,
                NurseName =
                    complaint.Nurse == null
                        ? "-"
                        : complaint.Nurse.User.FirstName + " " +
                          complaint.Nurse.User.LastName,
                Title = complaint.Title,
                Description = complaint.Description,
                Status = complaint.Status,
                CreatedAt = complaint.CreatedAt,
                AdminNotes = complaint.AdminNotes
            };
        }

        // ==========================================
        // Resolve
        // ==========================================

        public async Task<ServiceResult> ResolveComplaintAsync(int id)
        {
            var complaint = await _context.Complaints
                .Include(x => x.Patient)
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

            await _notificationService.CreateAsync(
                complaint.PatientId,
                "Patient",
                "Complaint Resolved",
                "Your complaint has been reviewed and resolved.",
                "ComplaintResolved");

            return new ServiceResult
            {
                Success = true,
                Message = "Complaint resolved successfully."
            };
        }
    }
}