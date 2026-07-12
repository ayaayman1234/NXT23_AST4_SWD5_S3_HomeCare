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
        private readonly INotificationService _notificationService;

        public AdminService(
            NursingDbContext context,
            INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;


        }
        //=====================================================
        // Dashboard
        //=====================================================

        public async Task<AdminDashboardViewModel> GetDashboardAsync()
        {
            return new AdminDashboardViewModel
            {
                TotalPatients = await _context.Patients.CountAsync(),

                TotalNurses = await _context.Nurses.CountAsync(),

                TotalAdmins = await _context.Admins.CountAsync(),

                TotalCareRequests = await _context.CareRequests.CountAsync(),

                PendingRequests = await _context.CareRequests
                    .CountAsync(x => x.RequestStatus == "Pending"),

                AcceptedRequests = await _context.CareRequests
                    .CountAsync(x => x.RequestStatus == "Accepted"),

                CompletedRequests = await _context.CareRequests
                    .CountAsync(x => x.RequestStatus == "Completed"),

                TotalComplaints = await _context.Complaints.CountAsync(),

                OpenComplaints = await _context.Complaints
                    .CountAsync(x => x.Status == "Open"),

                TotalSOS = await _context.SOSEvents.CountAsync(),

                PendingSOS = await _context.SOSEvents
                    .CountAsync(x => x.SOSStatus == "Pending"),

                TotalCancellations = await _context.Cancellations.CountAsync(),

                TotalPayments = await _context.Payments.CountAsync(),

                TotalRevenue = await _context.Payments
                    .SumAsync(x => (decimal?)x.Amount) ?? 0,

                AverageRating = await _context.Ratings.AnyAsync()
                    ? await _context.Ratings.AverageAsync(x => x.RatingScore)
                    : 0
            };
        }
        //=====================================================
        // Get All Nurses
        //=====================================================

        public async Task<List<AdminNurseViewModel>> GetAllNursesAsync()
        {
            return await _context.Nurses
                .Include(x => x.User)
                .OrderBy(x => x.User.FirstName)
                .Select(x => new AdminNurseViewModel
                {
                    Id = x.Id,

                    FullName = x.User.FirstName + " " + x.User.LastName,

                    Email = x.User.Email!,

                    PhoneNumber = x.User.PhoneNumber!,

                    Specialization = x.Specialization,

                    YearsExperience = x.YearsExperience,

                    IsVerified = x.IsVerified,

                    IsAvailable = x.IsAvailable
                })
                .ToListAsync();
        }
        //=====================================================
        // Nurse Details
        //=====================================================

        public async Task<AdminNurseViewModel?> GetNurseDetailsAsync(int nurseId)
        {
            return await _context.Nurses
                .Include(x => x.User)
                .Where(x => x.Id == nurseId)
                .Select(x => new AdminNurseViewModel
                {
                    Id = x.Id,

                    FullName = x.User.FirstName + " " + x.User.LastName,

                    Email = x.User.Email!,

                    PhoneNumber = x.User.PhoneNumber!,

                    Specialization = x.Specialization,

                    YearsExperience = x.YearsExperience,

                    IsVerified = x.IsVerified,

                    IsAvailable = x.IsAvailable
                })
                .FirstOrDefaultAsync();
        }
        //=====================================================
        // Approve Nurse
        //=====================================================

        public async Task<ServiceResult> ApproveNurseAsync(int nurseId)
        {
            var nurse = await _context.Nurses
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == nurseId);

            if (nurse == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Nurse not found."
                };
            }

            // Business Logic Rule: Nurse must have uploaded at least one document
            var hasDocuments = await _context.NurseDocuments.AnyAsync(x => x.NurseId == nurseId);
            if (!hasDocuments)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Cannot approve this nurse. The nurse must upload at least one verification document (e.g., ID or license) before approval."
                };
            }

            nurse.IsVerified = true;

            await _context.SaveChangesAsync();

            await _notificationService.CreateAsync(
                nurse.Id,
                "Nurse",
                "Account Approved",
                "Congratulations! Your account has been approved.",
                "AccountApproved");

            return new ServiceResult
            {
                Success = true,
                Message = "Nurse approved successfully."
            };
        }
        //=====================================================
        // Reject Nurse
        //=====================================================

        public async Task<ServiceResult> RejectNurseAsync(int nurseId)
        {
            var nurse = await _context.Nurses
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == nurseId);

            if (nurse == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Nurse not found."
                };
            }

            await _notificationService.CreateAsync(
                nurse.Id,
                "Nurse",
                "Account Rejected",
                "Unfortunately your account has been rejected.",
                "AccountRejected");

            _context.Nurses.Remove(nurse);

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Nurse rejected successfully."
            };
        }
        //=====================================================
        // Block Nurse
        //=====================================================

        public async Task<ServiceResult> BlockNurseAsync(int nurseId)
        {
            var nurse = await _context.Nurses
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == nurseId);

            if (nurse == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Nurse not found."
                };
            }

            nurse.User.AccountStatus = "Blocked";

            await _context.SaveChangesAsync();

            await _notificationService.CreateAsync(
                nurse.Id,
                "Nurse",
                "Account Blocked",
                "Your account has been blocked by the administrator.",
                "AccountBlocked");

            return new ServiceResult
            {
                Success = true,
                Message = "Nurse blocked successfully."
            };
        }
        //=====================================================
        // UnBlock Nurse
        //=====================================================

        public async Task<ServiceResult> UnBlockNurseAsync(int nurseId)
        {
            var nurse = await _context.Nurses
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == nurseId);

            if (nurse == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Nurse not found."
                };
            }

            nurse.User.AccountStatus = "Active";

            await _context.SaveChangesAsync();

            await _notificationService.CreateAsync(
                nurse.Id,
                "Nurse",
                "Account Activated",
                "Your account has been activated again.",
                "AccountActivated");

            return new ServiceResult
            {
                Success = true,
                Message = "Nurse activated successfully."
            };
        }

        //=====================================================
        // Delete Nurse (with Business Logic)
        //=====================================================

        public async Task<ServiceResult> DeleteNurseAsync(int nurseId)
        {
            // Business Logic: cannot delete nurse with active care requests
            var hasActiveRequests = await _context.CareRequests
                .AnyAsync(x => x.NurseId == nurseId &&
                         (x.RequestStatus == "Pending" || x.RequestStatus == "Accepted"));

            if (hasActiveRequests)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Cannot delete this nurse because they have active care requests. Please reassign or close the requests first."
                };
            }

            var nurse = await _context.Nurses
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == nurseId);

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
                Message = "Nurse deleted successfully."
            };
        }

        //=====================================================
        // Nurse Documents
        //=====================================================

        public async Task<List<AdminDocumentViewModel>> GetNurseDocumentsAsync(int nurseId)
        {
            

            return await _context.NurseDocuments
                .Where(x => x.NurseId == nurseId)
                .Include(x => x.Nurse)
                    .ThenInclude(x => x.User)
                .Select(x => new AdminDocumentViewModel
                {
                    Id = x.Id,

                    NurseId = x.NurseId,

                    NurseName = (x.Nurse != null && x.Nurse.User != null)
                        ? x.Nurse.User.FirstName + " " + x.Nurse.User.LastName
                        : "-",

                    DocumentType = x.DocumentType,

                    FilePath = x.FilePath,

                    UploadedAt = x.UploadDate,

                    IsApproved = x.IsApproved,

                    // Pull expiry date from the most recent Verification record
                    ExpiryDate = _context.Verifications
                        .Where(v => v.NurseDocumentId == x.Id)
                        .OrderByDescending(v => v.VerificationTime)
                        .Select(v => v.ExpiryDate)
                        .FirstOrDefault(),

                    VerificationStatus = _context.Verifications
                        .Where(v => v.NurseDocumentId == x.Id)
                        .OrderByDescending(v => v.VerificationTime)
                        .Select(v => v.VerificationStatus)
                        .FirstOrDefault() ?? "Pending"
                })
                .ToListAsync();
        }

        //=====================================================
        // Approve Document
        //=====================================================

        public async Task<ServiceResult> ApproveDocumentAsync(int documentId)
        {
            var document = await _context.NurseDocuments
                .Include(x => x.Nurse)
                .FirstOrDefaultAsync(x => x.Id == documentId);

            if (document == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Document not found."
                };
            }

            document.IsApproved = true;

            await _context.SaveChangesAsync();

            await _notificationService.CreateAsync(
                document.Nurse.Id,
                "Nurse",
                "Document Approved",
                "Your document has been approved successfully.",
                "DocumentApproved");

            return new ServiceResult
            {
                Success = true,
                Message = "Document approved successfully."
            };
        }
        //=====================================================
        // Reject Document
        //=====================================================

        public async Task<ServiceResult> RejectDocumentAsync(int documentId)
        {
            var document = await _context.NurseDocuments
                .Include(x => x.Nurse)
                .FirstOrDefaultAsync(x => x.Id == documentId);

            if (document == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Document not found."
                };
            }

            document.IsApproved = false;

            await _context.SaveChangesAsync();

            await _notificationService.CreateAsync(
                document.Nurse.Id,
                "Nurse",
                "Document Rejected",
                "Your document has been rejected by the administrator.",
                "DocumentRejected");

            return new ServiceResult
            {
                Success = true,
                Message = "Document rejected successfully."
            };
        }
        //=====================================================
        // Get All Patients
        //=====================================================

        public async Task<List<AdminPatientViewModel>> GetAllPatientsAsync()
        {
            return await _context.Patients
                .Include(x => x.User)
                .Select(x => new AdminPatientViewModel
                {
                    Id = x.Id,

                    FullName = x.User.FirstName + " " + x.User.LastName,

                    Email = x.User.Email!,

                    PhoneNumber = x.User.PhoneNumber!,

                    Address = x.User.Address,

                    Governorate = x.User.Governorate,

                    City = x.User.City,

                    Age = x.User.Age,

                    Gender = x.User.Gender,

                    BloodType = x.BloodType ?? "-",

                    MedicalHistory = x.MedicalHistory,

                    IsBlocked = x.User.AccountStatus == "Blocked"
                })
                .ToListAsync();
        }
        //=====================================================
        // Patient Details
        //=====================================================

        public async Task<AdminPatientViewModel?> GetPatientDetailsAsync(int patientId)
        {
            return await _context.Patients
                .Include(x => x.User)
                .Where(x => x.Id == patientId)
                .Select(x => new AdminPatientViewModel
                {
                    Id = x.Id,

                    FullName = x.User.FirstName + " " + x.User.LastName,

                    Email = x.User.Email!,

                    PhoneNumber = x.User.PhoneNumber!,

                    Address = x.User.Address,

                    Governorate = x.User.Governorate,

                    City = x.User.City,

                    Age = x.User.Age,

                    Gender = x.User.Gender,

                    BloodType = x.BloodType ?? "-",

                    MedicalHistory = x.MedicalHistory,

                    IsBlocked = x.User.AccountStatus == "Blocked"
                })
                .FirstOrDefaultAsync();
        }
        //=====================================================
        // Block Patient
        //=====================================================

        public async Task<ServiceResult> BlockPatientAsync(int patientId)
        {
            var patient = await _context.Patients
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == patientId);

            if (patient == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Patient not found."
                };
            }

            patient.User.AccountStatus = "Blocked";

            await _context.SaveChangesAsync();

            await _notificationService.CreateAsync(
                patient.Id,
                "Patient",
                "Account Blocked",
                "Your account has been blocked by the administrator.",
                "AccountBlocked");

            return new ServiceResult
            {
                Success = true,
                Message = "Patient blocked successfully."
            };
        }

        //=====================================================
        // UnBlock Patient
        //=====================================================

        public async Task<ServiceResult> UnBlockPatientAsync(int patientId)
        {
            var patient = await _context.Patients
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == patientId);

            if (patient == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Patient not found."
                };
            }

            patient.User.AccountStatus = "Active";

            await _context.SaveChangesAsync();

            await _notificationService.CreateAsync(
                patient.Id,
                "Patient",
                "Account Activated",
                "Your account has been reactivated by the administrator.",
                "AccountActivated");

            return new ServiceResult
            {
                Success = true,
                Message = "Patient unblocked successfully."
            };
        }

        public async Task<ServiceResult> DeletePatientAsync(int patientId)
        {
            var patient = await _context.Patients
                .FirstOrDefaultAsync(x => x.Id == patientId);

            if (patient == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Patient not found."
                };
            }

            _context.Patients.Remove(patient);

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Patient deleted successfully."
            };
        }
        //=====================================================
        // Get All Care Requests
        //=====================================================

        public async Task<List<AdminCareRequestViewModel>> GetAllCareRequestsAsync()
        {
            return await _context.CareRequests
                .Include(x => x.Patient)
                    .ThenInclude(x => x.User)
                .Include(x => x.Nurse)
                    .ThenInclude(x => x.User)
                .Include(x => x.Service)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new AdminCareRequestViewModel
                {
                    Id = x.Id,

                    PatientName =
                        x.Patient.User.FirstName + " " +
                        x.Patient.User.LastName,

                    NurseName =
                        x.Nurse == null
                        ? "-"
                        : x.Nurse.User.FirstName + " " +
                          x.Nurse.User.LastName,

                    ServiceName = x.Service.Name,

                    Address = x.Address,

                    Description = x.Description,

                    PreferredDate = x.PreferredDate,

                    RequiredHours = x.RequiredHours,

                    BudgetMin = x.BudgetMin,

                    BudgetMax = x.BudgetMax,

                    RequestPriority = x.RequestPriority,

                    Status = x.RequestStatus
                })
                .ToListAsync();
        }
        //=====================================================
        // Care Request Details
        //=====================================================

        public async Task<AdminCareRequestViewModel?> GetCareRequestDetailsAsync(int requestId)
        {
            return await _context.CareRequests
                .Include(x => x.Patient)
                    .ThenInclude(x => x.User)
                .Include(x => x.Nurse)
                    .ThenInclude(x => x.User)
                .Include(x => x.Service)
                .Where(x => x.Id == requestId)
                .Select(x => new AdminCareRequestViewModel
                {
                    Id = x.Id,

                    PatientName =
                        x.Patient.User.FirstName + " " +
                        x.Patient.User.LastName,

                    NurseName =
                        x.Nurse == null
                        ? "-"
                        : x.Nurse.User.FirstName + " " +
                          x.Nurse.User.LastName,

                    ServiceName = x.Service.Name,

                    Address = x.Address,

                    Description = x.Description,

                    PreferredDate = x.PreferredDate,

                    RequiredHours = x.RequiredHours,

                    BudgetMin = x.BudgetMin,

                    BudgetMax = x.BudgetMax,

                    RequestPriority = x.RequestPriority,

                    Status = x.RequestStatus
                })
                .FirstOrDefaultAsync();
        }
        //=====================================================
        // Delete Care Request
        //=====================================================

        public async Task<ServiceResult> DeleteCareRequestAsync(int requestId)
        {
            var request = await _context.CareRequests
                .FirstOrDefaultAsync(x => x.Id == requestId);

            if (request == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Care Request not found."
                };
            }

            _context.CareRequests.Remove(request);

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Care Request deleted successfully."
            };
        }
        //=====================================================
        // Get All Complaints
        //=====================================================

        public async Task<List<AdminComplaintViewModel>> GetAllComplaintsAsync()
        {
            return await _context.Complaints
                .Include(x => x.Patient)
                    .ThenInclude(x => x.User)
                .Include(x => x.Nurse)
                    .ThenInclude(x => x.User)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new AdminComplaintViewModel
                {
                    Id = x.Id,

                    PatientName =
                        x.Patient.User.FirstName + " " +
                        x.Patient.User.LastName,

                    NurseName =
                        x.Nurse == null
                        ? "-"
                        : x.Nurse.User.FirstName + " " +
                          x.Nurse.User.LastName,

                    Title = x.Title,

                    Description = x.Description,

                    Status = x.Status,

                    CreatedAt = x.CreatedAt,

                    AdminNotes = x.AdminNotes
                })
                .ToListAsync();
        }
        //=====================================================
        // Complaint Details
        //=====================================================

        public async Task<AdminComplaintViewModel?> GetComplaintAsync(int complaintId)
        {
            return await _context.Complaints
                .Include(x => x.Patient)
                    .ThenInclude(x => x.User)
                .Include(x => x.Nurse)
                    .ThenInclude(x => x.User)
                .Where(x => x.Id == complaintId)
                .Select(x => new AdminComplaintViewModel
                {
                    Id = x.Id,

                    PatientName =
                        x.Patient.User.FirstName + " " +
                        x.Patient.User.LastName,

                    NurseName =
                        x.Nurse == null
                        ? "-"
                        : x.Nurse.User.FirstName + " " +
                          x.Nurse.User.LastName,

                    Title = x.Title,

                    Description = x.Description,

                    Status = x.Status,

                    CreatedAt = x.CreatedAt,

                    AdminNotes = x.AdminNotes
                })
                .FirstOrDefaultAsync();
        }
        //=====================================================
        // Resolve Complaint
        //=====================================================

        public async Task<ServiceResult> ResolveComplaintAsync(
            int complaintId,
            string? adminNotes)
        {
            var complaint = await _context.Complaints
                .Include(x => x.Patient)
                .FirstOrDefaultAsync(x => x.Id == complaintId);

            if (complaint == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Complaint not found."
                };
            }

            complaint.Status = "Resolved";

            complaint.AdminNotes = adminNotes;

            await _context.SaveChangesAsync();

            await _notificationService.CreateAsync(
                complaint.Patient.Id,
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
        //=====================================================
        // Reject Complaint
        //=====================================================

        public async Task<ServiceResult> RejectComplaintAsync(
            int complaintId,
            string? adminNotes)
        {
            var complaint = await _context.Complaints
                .Include(x => x.Patient)
                .FirstOrDefaultAsync(x => x.Id == complaintId);

            if (complaint == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Complaint not found."
                };
            }

            complaint.Status = "Rejected";

            complaint.AdminNotes = adminNotes;

            await _context.SaveChangesAsync();

            await _notificationService.CreateAsync(
                complaint.Patient.Id,
                "Patient",
                "Complaint Rejected",
                "Your complaint has been reviewed and rejected.",
                "ComplaintRejected");

            return new ServiceResult
            {
                Success = true,
                Message = "Complaint rejected successfully."
            };
        }
        public async Task<List<AdminSOSViewModel>> GetAllSOSAsync()
        {
            return await _context.SOSEvents
                .Include(x => x.TriggeredByUser)
                    .ThenInclude(x => x.User)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new AdminSOSViewModel
                {
                    Id = x.Id,

                    PatientName =
                        x.TriggeredByUser.User.FirstName + " " +
                        x.TriggeredByUser.User.LastName,

                    Location = x.Location,

                    CreatedAt = x.CreatedAt,

                    Status = x.SOSStatus
                })
                .ToListAsync();
        }
        public async Task<ServiceResult> ResolveSOSAsync(int sosId)
        {
            var sos = await _context.SOSEvents
                .FirstOrDefaultAsync(x => x.Id == sosId);

            if (sos == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "SOS Request not found."
                };
            }

            sos.SOSStatus = "Resolved";

            await _context.SaveChangesAsync();

            // Notify the patient who triggered the SOS
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.Id == sos.TriggeredByUserId);

            if (patient != null)
            {
                await _notificationService.CreateAsync(
                    patient.Id,
                    "Patient",
                    "SOS Resolved",
                    "Your SOS request has been resolved by the administrator.",
                    "SOSResolved");
            }

            return new ServiceResult
            {
                Success = true,
                Message = "SOS Request resolved successfully."
            };
        }
        public async Task<List<AdminCancellationViewModel>> GetAllCancellationsAsync()
        {
            return await _context.Cancellations
                .OrderByDescending(x => x.RequestedAt)
                .Select(x => new AdminCancellationViewModel
                {
                    Id = x.Id,

                    CareRequestId = x.CareRequestId,

                    RequestedBy = x.RequestedById.ToString(),

                    RequestedByType = x.RequestedByType,

                    Reason = x.Reason,

                    Fee = x.Fee,

                    RequestedAt = x.RequestedAt,

                    Status = x.Status
                })
                .ToListAsync();
        }
        public async Task<ServiceResult> ApproveCancellationAsync(int cancellationId)
        {
            var cancellation = await _context.Cancellations
                .FirstOrDefaultAsync(x => x.Id == cancellationId);

            if (cancellation == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Cancellation request not found."
                };
            }

            cancellation.Status = "Approved";

            await _context.SaveChangesAsync();

            // Notify the person who requested the cancellation
            await _notificationService.CreateAsync(
                cancellation.RequestedById,
                cancellation.RequestedByType,
                "Cancellation Approved",
                "Your cancellation request has been approved by the administrator.",
                "CancellationApproved");

            return new ServiceResult
            {
                Success = true,
                Message = "Cancellation approved successfully."
            };
        }
        public async Task<ServiceResult> RejectCancellationAsync(int cancellationId)
        {
            var cancellation = await _context.Cancellations
                .FirstOrDefaultAsync(x => x.Id == cancellationId);

            if (cancellation == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Cancellation request not found."
                };
            }

            cancellation.Status = "Rejected";

            await _context.SaveChangesAsync();

            // Notify the person who requested the cancellation
            await _notificationService.CreateAsync(
                cancellation.RequestedById,
                cancellation.RequestedByType,
                "Cancellation Rejected",
                "Your cancellation request has been rejected by the administrator.",
                "CancellationRejected");

            return new ServiceResult
            {
                Success = true,
                Message = "Cancellation rejected successfully."
            };
        }
        public async Task<List<AdminPaymentViewModel>> GetAllPaymentsAsync()
        {
            return await _context.Payments
                .OrderByDescending(x => x.PaymentDate)
                .Select(x => new AdminPaymentViewModel
                {
                    Id = x.Id,

                    CareRequestId = x.CareRequestId,

                    Amount = x.Amount,

                    CommissionAmount = x.CommissionAmount,

                    NetAmount = x.NetAmount,

                    PaymentMethod = x.PaymentMethod,

                    PaymentStatus = x.PaymentStatus,

                    TransactionReference = x.TransactionReference,

                    PaymentDate = x.PaymentDate
                })
                .ToListAsync();
        }
        public async Task<AdminPaymentViewModel?> GetPaymentDetailsAsync(int paymentId)
        {
            return await _context.Payments
                .Where(x => x.Id == paymentId)
                .Select(x => new AdminPaymentViewModel
                {
                    Id = x.Id,

                    CareRequestId = x.CareRequestId,

                    Amount = x.Amount,

                    CommissionAmount = x.CommissionAmount,

                    NetAmount = x.NetAmount,

                    PaymentMethod = x.PaymentMethod,

                    PaymentStatus = x.PaymentStatus,

                    TransactionReference = x.TransactionReference,

                    PaymentDate = x.PaymentDate
                })
                .FirstOrDefaultAsync();
        }
        ////=====================================================
        //// Get All Ratings
        ////=====================================================

        //public async Task<List<Rating>> GetAllRatingsAsync()
        //{
        //    return await _context.Ratings
        //        .Include(x => x.CareRequest)
        //        .OrderByDescending(x => x.CreatedAt)
        //        .ToListAsync();
        //}

        //=====================================================
        // Get All Ratings
        //=====================================================

        public async Task<List<AdminRatingViewModel>> GetAllRatingsAsync()
        {
            return await _context.Ratings
                .Include(x => x.CareRequest)
                    .ThenInclude(x => x.Patient)
                        .ThenInclude(x => x.User)
                .Include(x => x.CareRequest)
                    .ThenInclude(x => x.Nurse)
                        .ThenInclude(x => x.User)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new AdminRatingViewModel
                {
                    Id = x.Id,

                    CareRequestId = x.CareRequestId,

                    PatientName =
                        x.CareRequest.Patient.User.FirstName + " " +
                        x.CareRequest.Patient.User.LastName,

                    NurseName =
                        x.CareRequest.Nurse == null
                        ? "-"
                        : x.CareRequest.Nurse.User.FirstName + " " +
                          x.CareRequest.Nurse.User.LastName,

                    RatingScore = x.RatingScore,

                    Comment = x.RatingComment ?? "",

                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();
        }

        //=====================================================
        // Get Report
        //=====================================================

        public async Task<AdminReportViewModel> GetReportAsync()
        {
            return new AdminReportViewModel
            {
                TotalPatients = await _context.Patients.CountAsync(),

                TotalNurses = await _context.Nurses.CountAsync(),

                TotalAdmins = await _context.Admins.CountAsync(),

                TotalCareRequests = await _context.CareRequests.CountAsync(),

                PendingRequests = await _context.CareRequests
                    .CountAsync(x => x.RequestStatus == "Pending"),

                AcceptedRequests = await _context.CareRequests
                    .CountAsync(x => x.RequestStatus == "Accepted"),

                CompletedRequests = await _context.CareRequests
                    .CountAsync(x => x.RequestStatus == "Completed"),

                TotalRevenue = await _context.Payments
                    .SumAsync(x => (decimal?)x.Amount) ?? 0,

                TotalCommission = await _context.Payments
                    .SumAsync(x => (decimal?)x.CommissionAmount) ?? 0,

                TotalPayments = await _context.Payments.CountAsync(),

                TotalRatings = await _context.Ratings.CountAsync(),

                AverageRating = await _context.Ratings.AnyAsync()
                    ? await _context.Ratings.AverageAsync(x => x.RatingScore)
                    : 0,

                TotalComplaints = await _context.Complaints.CountAsync(),

                OpenComplaints = await _context.Complaints
                    .CountAsync(x => x.Status == "Open"),

                ResolvedComplaints = await _context.Complaints
                    .CountAsync(x => x.Status == "Resolved"),

                TotalSOS = await _context.SOSEvents.CountAsync(),

                PendingSOS = await _context.SOSEvents
                    .CountAsync(x => x.SOSStatus == "Pending"),

                TotalCancellations = await _context.Cancellations.CountAsync()
            };
        }

        //=====================================================
        // Get Admin Profile
        //=====================================================

        public async Task<AdminProfileViewModel?> GetProfileAsync(string userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null) return null;

            return new AdminProfileViewModel
            {
                FirstName = user.FirstName,

                LastName = user.LastName,

                Email = user.Email ?? "",

                PhoneNumber = user.PhoneNumber,

                ProfilePhoto = user.ProfilePhoto,

                CreatedAt = user.CreatedAt
            };
        }

        //=====================================================
        // Update Admin Profile
        //=====================================================

        public async Task<ServiceResult> UpdateProfileAsync(
            string userId,
            AdminProfileViewModel model)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            user.FirstName = model.FirstName;

            user.LastName = model.LastName;

            user.PhoneNumber = model.PhoneNumber;

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Profile updated successfully."
            };
        }

        //=====================================================
        // Get All Notifications (System-wide)
        //=====================================================

        public async Task<List<AdminNotificationViewModel>> GetAllNotificationsAsync()
        {
            return await _context.Notifications
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new AdminNotificationViewModel
                {
                    Id = x.Id,

                    Title = x.Title,

                    Message = x.Message,

                    NotificationType = x.NotificationType,

                    ReceiverType = x.ReceiverType,

                    ReceiverId = x.ReceiverId,

                    IsRead = x.IsRead,

                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();
        }

        //=====================================================
        // Mark Notification as Read
        //=====================================================

        public async Task<ServiceResult> MarkNotificationReadAsync(int notificationId)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(x => x.Id == notificationId);

            if (notification == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Notification not found."
                };
            }

            notification.IsRead = true;

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Notification marked as read."
            };
        }
    }
}
