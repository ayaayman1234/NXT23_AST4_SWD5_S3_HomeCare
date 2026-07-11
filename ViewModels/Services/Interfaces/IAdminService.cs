using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.ViewModels.Admin;

namespace NursingCarePlatform.Web.Services.Interfaces
{
    public interface IAdminService
    {
        // ==========================
        // Dashboard
        // ==========================

        Task<AdminDashboardViewModel> GetDashboardAsync();

        // ==========================
        // Nurse Management
        // ==========================

        Task<List<AdminNurseViewModel>> GetAllNursesAsync();

        Task<AdminNurseViewModel?> GetNurseDetailsAsync(int nurseId);

        Task<ServiceResult> ApproveNurseAsync(int nurseId);

        Task<ServiceResult> RejectNurseAsync(int nurseId);

        Task<ServiceResult> BlockNurseAsync(int nurseId);

        Task<ServiceResult> UnBlockNurseAsync(int nurseId);

        Task<ServiceResult> DeleteNurseAsync(int nurseId);

        // ==========================
        // Patient Management
        // ==========================

        Task<List<AdminPatientViewModel>> GetAllPatientsAsync();

        Task<AdminPatientViewModel?> GetPatientDetailsAsync(int patientId);

        Task<ServiceResult> BlockPatientAsync(int patientId);

        Task<ServiceResult> UnBlockPatientAsync(int patientId);

        Task<ServiceResult> DeletePatientAsync(int patientId);

        // ==========================
        // Care Requests
        // ==========================

        Task<List<AdminCareRequestViewModel>> GetAllCareRequestsAsync();

        Task<AdminCareRequestViewModel?> GetCareRequestDetailsAsync(int requestId);

        Task<ServiceResult> DeleteCareRequestAsync(int requestId);

        // ==========================
        // Complaints
        // ==========================

        Task<List<AdminComplaintViewModel>> GetAllComplaintsAsync();

        Task<AdminComplaintViewModel?> GetComplaintAsync(int complaintId);

        Task<ServiceResult> ResolveComplaintAsync(int complaintId, string? adminNotes);

        Task<ServiceResult> RejectComplaintAsync(int complaintId, string? adminNotes);

        // ==========================
        // Nurse Documents
        // ==========================

        Task<List<AdminDocumentViewModel>> GetNurseDocumentsAsync(int nurseId);

        Task<ServiceResult> ApproveDocumentAsync(int documentId);

        Task<ServiceResult> RejectDocumentAsync(int documentId);

        // ==========================
        // SOS
        // ==========================

        Task<List<AdminSOSViewModel>> GetAllSOSAsync();

        Task<ServiceResult> ResolveSOSAsync(int sosId);

        // ==========================
        // Cancellations
        // ==========================

        Task<List<AdminCancellationViewModel>> GetAllCancellationsAsync();

        Task<ServiceResult> ApproveCancellationAsync(int cancellationId);

        Task<ServiceResult> RejectCancellationAsync(int cancellationId);

        // ==========================
        // Payments
        // ==========================

        Task<List<AdminPaymentViewModel>> GetAllPaymentsAsync();

        Task<AdminPaymentViewModel?> GetPaymentDetailsAsync(int paymentId);

        // ==========================
        // Ratings
        // ==========================

        Task<List<AdminRatingViewModel>> GetAllRatingsAsync();

        // ==========================
        // Reports
        // ==========================

        Task<AdminReportViewModel> GetReportAsync();

        // ==========================
        // Profile
        // ==========================

        Task<AdminProfileViewModel?> GetProfileAsync(string userId);

        Task<ServiceResult> UpdateProfileAsync(string userId, AdminProfileViewModel model);

        // ==========================
        // Notifications
        // ==========================

        Task<List<AdminNotificationViewModel>> GetAllNotificationsAsync();

        Task<ServiceResult> MarkNotificationReadAsync(int notificationId);
    }
}