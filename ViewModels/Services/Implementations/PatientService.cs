using Microsoft.EntityFrameworkCore;
using NursingCarePlatform.Web.Data;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels.CareRequest;
using NursingCarePlatform.Web.ViewModels.Nurse;
using NursingCarePlatform.Web.ViewModels.Patient;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NursingCarePlatform.Web.Services.Implementations
{
    public class PatientService : IPatientService
    {
        private readonly NursingDbContext _context;
        private readonly INotificationService _notificationService;

        public PatientService(
            NursingDbContext context,
            INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        // ==========================================
        // Patient Profile
        // ==========================================

        public async Task<Patient?> GetPatientByUserIdAsync(string userId)
        {
            return await _context.Patients
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }
        // ==========================================
        // Nurse Profile with Ratings
        // ==========================================

        public async Task<NurseProfileViewModel?> GetProfileAsync(string userId)
        {
            var nurse = await _context.Nurses
                .Include(n => n.User)
                .Include(n => n.Documents)
                .Include(n => n.Availabilities)
                .Include(n => n.NurseServices)
                    .ThenInclude(ns => ns.Service)
                .FirstOrDefaultAsync(n => n.UserId == userId);

            if (nurse == null)
                return null;

            var ratings = await _context.Ratings
                .Where(r => r.RatedUserId == nurse.Id)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return new NurseProfileViewModel
            {
                Nurse = nurse,

                AverageRating = ratings.Any()
                    ? ratings.Average(r => r.RatingScore)
                    : 0,

                ReviewsCount = ratings.Count,

                Reviews = ratings
            };
        }

        public async Task<ServiceResult> UpdateProfileAsync(
    string userId,
    EditPatientViewModel model)
        {
            var patient = await _context.Patients
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (patient == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Patient not found."
                };
            }

            // ==========================
            // Update Identity User
            // ==========================

            patient.User.FirstName = model.FirstName;
            patient.User.LastName = model.LastName;

            patient.User.Email = model.Email;
            patient.User.UserName = model.Email;
            patient.User.NormalizedEmail = model.Email.ToUpper();
            patient.User.NormalizedUserName = model.Email.ToUpper();

            patient.User.PhoneNumber = model.PhoneNumber;

            patient.User.Gender = model.Gender ?? string.Empty;
            patient.User.Age = model.Age;

            patient.User.Address = model.Address ?? string.Empty;
            patient.User.City = model.City ?? string.Empty;
            patient.User.Governorate = model.Governorate ?? string.Empty;

            // ==========================
            // Update Patient
            // ==========================

            patient.BloodType = model.BloodType;

            patient.MedicalHistory = model.MedicalHistory ?? string.Empty;

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Profile updated successfully."
            };
        }

        // ==========================================
        // Create Care Request
        // ==========================================

        public async Task<ServiceResult> CreateRequestAsync(
            string userId,
            CreateCareRequestViewModel model)
        {
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (patient == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Patient not found."
                };
            }

            var request = new CareRequest
            {
                PatientId = patient.Id,
                ServiceId = model.ServiceId,

                Address = model.Address,
                Description = model.Description,

                PreferredDate = model.PreferredDate,

                RequiredHours = model.RequiredHours,

                OvernightStay = model.OvernightStay,

                AccommodationAvailable = model.AccommodationAvailable,

                BudgetMin = model.BudgetMin,

                BudgetMax = model.BudgetMax,

                PreferredNurseGender = model.PreferredNurseGender,

                RequestPriority = model.RequestPriority,

                MatchingType = model.MatchingType,

                RequestStatus = "Pending",

                CreatedAt = DateTime.Now
            };

            _context.CareRequests.Add(request);

            await _context.SaveChangesAsync();
            await _notificationService.CreateAsync(
    userId,
    "Care Request",
    "Your care request has been created successfully.",
    "CareRequestCreated"
);

            return new ServiceResult
            {
                Success = true,
                Message = "Care Request created successfully."
            };
        }

        // ==========================================
        // Get All My Requests
        // ==========================================
        public async Task<List<SelectListItem>> GetServicesAsync()
        {
            return await _context.NursingServices
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToListAsync();
        }
        public async Task<List<CareRequest>> GetMyRequestsAsync(string userId)
        {
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (patient == null)
            {
                return new List<CareRequest>();
            }

            return await _context.CareRequests
                .Include(c => c.Service)
                .Where(c => c.PatientId == patient.Id)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        // ==========================================
        // Get Request Details
        // ==========================================

        public async Task<CareRequest?> GetRequestByIdAsync(int requestId)
        {
            return await _context.CareRequests
                .Include(c => c.Service)
                .Include(c => c.Patient)
                .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(c => c.Id == requestId);
        }

        // ==========================================
        // Edit Request
        // ==========================================

        public async Task<ServiceResult> UpdateRequestAsync(
            EditCareRequestViewModel model)
        {
            var request = await _context.CareRequests
                .FirstOrDefaultAsync(c => c.Id == model.Id);

            if (request == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Request not found."
                };
            }

            request.ServiceId = model.ServiceId;
            request.Address = model.Address;
            request.Description = model.Description;
            request.PreferredDate = model.PreferredDate;
            request.BudgetMin = model.BudgetMin;
            request.BudgetMax = model.BudgetMax;

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Request updated successfully."
            };
        }

        // ==========================================
        // Delete Request
        // ==========================================

        public async Task<ServiceResult> DeleteRequestAsync(int requestId)
        {
            var request = await _context.CareRequests
                .FirstOrDefaultAsync(c => c.Id == requestId);

            if (request == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Request not found."
                };
            }

            _context.CareRequests.Remove(request);

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Request deleted successfully."
            };
        }

        // ==========================================
        // Cancel Request
        // ==========================================

        public async Task<ServiceResult> CancelRequestAsync(int requestId)
        {
            var request = await _context.CareRequests
                .FirstOrDefaultAsync(c => c.Id == requestId);

            if (request == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Request not found."
                };
            }

            request.RequestStatus = "Cancelled";

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Request cancelled successfully."
            };
        }
    }
}