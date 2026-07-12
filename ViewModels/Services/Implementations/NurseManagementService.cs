using Microsoft.EntityFrameworkCore;
using NursingCarePlatform.Web.Data;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels.Nurse;

namespace NursingCarePlatform.Web.Services.Implementations
{
    public partial class NurseManagementService : INurseService
    {
        private readonly NursingDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly INotificationService _notificationService;

        public NurseManagementService(
            NursingDbContext context,
            IWebHostEnvironment environment,
            INotificationService notificationService)
        {
            _context = context;
            _environment = environment;
            _notificationService = notificationService;
        }

        // ==========================================
        // Get Nurse Profile
        // ==========================================

        public async Task<Nurse?> GetNurseByUserIdAsync(string userId)
        {
            return await _context.Nurses
                .Include(n => n.User)
                .Include(n => n.Documents)
                .Include(n => n.Availabilities)
                .Include(n => n.NurseServices)
                .ThenInclude(ns => ns.Service)
                .FirstOrDefaultAsync(n => n.UserId == userId);
        }

        // ==========================================
        // Update Profile
        // ==========================================

        public async Task<ServiceResult> UpdateProfileAsync(
            string userId,
            EditNurseViewModel model)
        {
            var nurse = await _context.Nurses
                .Include(n => n.User)
                .FirstOrDefaultAsync(n => n.UserId == userId);

            if (nurse == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Nurse not found."
                };
            }

            // Identity User

            nurse.User.FirstName = model.FirstName;
            nurse.User.LastName = model.LastName;

            nurse.User.Email = model.Email;
            nurse.User.UserName = model.Email;

            nurse.User.PhoneNumber = model.PhoneNumber;

            nurse.User.Address = model.Address ?? "";
            nurse.User.City = model.City ?? "";
            nurse.User.Governorate = model.Governorate ?? "";

            nurse.User.Gender = model.Gender ?? "";
            nurse.User.Age = model.Age;

            // Nurse

            nurse.YearsExperience = model.YearsExperience;
            nurse.Specialization = model.Specialization;

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Profile updated successfully."
            };
        }

        // ==========================================
        // Availability
        // ==========================================

        public async Task<List<Availability>> GetAvailabilityAsync(string userId)
        {
            var nurse = await _context.Nurses
                .FirstOrDefaultAsync(n => n.UserId == userId);

            if (nurse == null)
                return new();

            return await _context.Availabilities
                .Where(a => a.NurseId == nurse.Id)
                .OrderBy(a => a.Day)
                .ToListAsync();
        }

        public async Task<ServiceResult> AddAvailabilityAsync(
            string userId,
            AvailabilityViewModel model)
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

            // 1. Validation: EndTime must be after StartTime
            if (model.StartTime >= model.EndTime)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "End time must be after start time."
                };
            }

            // 2. Validation: Prevent overlapping availability slots for the same nurse on the same day
            var hasOverlap = await _context.Availabilities
                .AnyAsync(a => a.NurseId == nurse.Id &&
                               a.Day == model.Day &&
                               model.StartTime < a.EndTime &&
                               model.EndTime > a.StartTime);

            if (hasOverlap)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "This time slot overlaps with an existing slot on the same day."
                };
            }

            var availability = new Availability
            {
                NurseId = nurse.Id,
                Day = model.Day,
                StartTime = model.StartTime,
                EndTime = model.EndTime
            };

            _context.Availabilities.Add(availability);

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Availability added successfully."
            };
        }

        public async Task<ServiceResult> DeleteAvailabilityAsync(int id)
        {
            var availability = await _context.Availabilities
                .FirstOrDefaultAsync(a => a.Id == id);

            if (availability == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Availability not found."
                };
            }

            _context.Availabilities.Remove(availability);

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Availability deleted successfully."
            };
        }
        // ==========================================
        // Documents
        // ==========================================

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

        public async Task<ServiceResult> UploadDocumentAsync(
            string userId,
            DocumentViewModel model)
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

            if (model.File == null || model.File.Length == 0)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Please choose a file."
                };
            }

            var folder = Path.Combine(
                _environment.WebRootPath,
                "uploads",
                "nurse-documents");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var extension = Path.GetExtension(model.File.FileName);

            var fileName = $"{Guid.NewGuid()}{extension}";

            var filePath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }

            var document = new NurseDocument
            {
                NurseId = nurse.Id,
                DocumentType = model.DocumentType,
                FilePath = fileName,
                UploadDate = DateTime.Now
            };

            _context.NurseDocuments.Add(document);

            await _context.SaveChangesAsync();

            // Send notification to all admins
            var admins = await _context.Admins.ToListAsync();
            foreach (var admin in admins)
            {
                await _notificationService.CreateAsync(
                    admin.Id,
                    "Admin",
                    "New Document Uploaded",
                    $"A nurse has uploaded a new document ({model.DocumentType}) for review.",
                    "DocumentUploaded");
            }

            return new ServiceResult
            {
                Success = true,
                Message = "Document uploaded successfully."
            };
        }

        public async Task<ServiceResult> DeleteDocumentAsync(int id)
        {
            var document = await _context.NurseDocuments
                .FirstOrDefaultAsync(d => d.Id == id);

            if (document == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Document not found."
                };
            }

            var path = Path.Combine(
                _environment.WebRootPath,
                "uploads",
                "nurse-documents",
                document.FilePath);

            if (File.Exists(path))
                File.Delete(path);

            _context.NurseDocuments.Remove(document);

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Document deleted successfully."
            };
        }
        // ==========================================
        // Services
        // ==========================================

        public async Task<List<NurseService>> GetServicesAsync(string userId)
{
    var nurse = await _context.Nurses
        .FirstOrDefaultAsync(x => x.UserId == userId);

    if (nurse == null)
        return new List<NurseService>();

            return await _context.NurseServices
            .Include(x => x.Service)
            .ThenInclude(s => s.Category)
            .Where(x => x.NurseId == nurse.Id)
            .ToListAsync();
        }

        public async Task<ServiceResult> AddServiceAsync(string userId, NurseServiceViewModel model)
        {
            var nurse = await _context.Nurses
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (nurse == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Nurse not found."
                };
            }

            var exists = await _context.NurseServices
                .AnyAsync(x => x.NurseId == nurse.Id &&
                               x.ServiceId == model.ServiceId);

            if (exists)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "This service has already been added."
                };
            }

            var nurseService = new NurseService
            {
                NurseId = nurse.Id,
                ServiceId = model.ServiceId,
                Price = model.Price
            };

            _context.NurseServices.Add(nurseService);

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Service added successfully."
            };
        }

        public async Task<ServiceResult> UpdateServiceAsync(
            string userId,
            NurseServiceViewModel model)
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

            var service = await _context.NurseServices
                .FirstOrDefaultAsync(x =>
                    x.NurseId == nurse.Id &&
                    x.ServiceId == model.ServiceId);

            if (service == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Service not found."
                };
            }

            service.Price = model.Price;

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Service updated successfully."
            };
        }

        public async Task<ServiceResult> DeleteServiceAsync(
            string userId,
            int serviceId)
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

            var service = await _context.NurseServices
                .FirstOrDefaultAsync(x =>
                    x.NurseId == nurse.Id &&
                    x.ServiceId == serviceId);

            if (service == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Service not found."
                };
            }

            _context.NurseServices.Remove(service);

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Service deleted successfully."
            };
        }
        public async Task<List<NursingService>> GetAllServicesAsync()
        {
            return await _context.NursingServices.ToListAsync();
        }
        public async Task<NurseProfileViewModel?> GetProfileAsync(string userId)
        {
            var nurse = await _context.Nurses
                .Include(n => n.User)
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
        public async Task<List<ServiceCategory>> GetCategoriesAsync()
        {
            return await _context.ServiceCategories
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<List<NursingService>> GetServicesByCategoryAsync(int categoryId)
        {
            return await _context.NursingServices
                .Where(s => s.CategoryId == categoryId)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }
        // ==========================================
        // Available Care Requests
        // ==========================================

        public async Task<List<CareRequest>> GetAvailableRequestsAsync(string userId)
        {
            var nurse = await _context.Nurses
                .FirstOrDefaultAsync(n => n.UserId == userId);

            if (nurse == null)
                return new List<CareRequest>();

            return await _context.CareRequests
                .Include(c => c.Patient)
                    .ThenInclude(p => p.User)
                .Include(c => c.Service)
                .Where(c =>
                    c.RequestStatus == "Pending"
                    && c.NurseId == null
                    && !_context.Offers.Any(o =>
                        o.CareRequestId == c.Id &&
                        o.NurseId == nurse.Id))
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }
    }

}
