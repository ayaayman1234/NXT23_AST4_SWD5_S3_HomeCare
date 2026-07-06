using Microsoft.EntityFrameworkCore;
using NursingCarePlatform.Web.Data;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels.WorkHistory;

namespace NursingCarePlatform.Web.Services.Implementations
{
    public class AssignmentService : IAssignmentService
    {
        private readonly NursingDbContext _context;
        private readonly INotificationService _notificationService;

        public AssignmentService(
            NursingDbContext context,
            INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task<List<Assignment>> GetNurseAssignmentsAsync(string userId)
        {
            var nurse = await _context.Nurses
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (nurse == null)
                return new List<Assignment>();

            var assignments = await _context.Assignments
    .Include(a => a.CareRequest)
        .ThenInclude(r => r.Patient)
            .ThenInclude(p => p.User)
    .Include(a => a.CareRequest.Service)
    .Where(a => a.NurseId == nurse.Id)
    .OrderByDescending(a => a.ShiftStart)
    .ToListAsync();

            foreach (var item in assignments)
            {
                item.CareRequest.MedicalChecklist =
                    await _context.MedicalChecklists
                        .FirstOrDefaultAsync(x => x.CareRequestId == item.CareRequest.Id);
            }

            return assignments;
        }

        public async Task<Assignment?> GetAssignmentAsync(int id)
        {
            return await _context.Assignments
                .Include(a => a.CareRequest)
                    .ThenInclude(c => c.Patient)
                        .ThenInclude(p => p.User)

                .Include(a => a.CareRequest)
                    .ThenInclude(c => c.Service)

                .Include(a => a.Nurse)
                    .ThenInclude(n => n.User)

                .Include(a => a.NursingNote)   

                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<ServiceResult> CompleteAssignmentAsync(int assignmentId)
        {
            var assignment = await _context.Assignments
                .Include(a => a.CareRequest)
                    .ThenInclude(c => c.Patient)
                .Include(a => a.Nurse)
                .FirstOrDefaultAsync(a => a.Id == assignmentId);

            if (assignment == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Assignment not found."
                };
            }

            assignment.AssignmentStatus = "Completed";

            assignment.CareRequest.RequestStatus = "Completed";
            assignment.CompletedAt = DateTime.Now;
            var history = new WorkHistory
            {
                NurseId = assignment.NurseId,

                PatientId = assignment.CareRequest.PatientId,

                CareRequestId = assignment.CareRequestId,

                ServiceId = assignment.CareRequest.ServiceId,

                CompletedAt = assignment.CompletedAt.Value,

                RequiredHours = assignment.CareRequest.RequiredHours,

                TotalAmount = assignment.CareRequest.BudgetMax
            };

            _context.WorkHistories.Add(history);

            await _context.SaveChangesAsync();

       
            var patientUserId = await _context.Patients
                .Where(p => p.Id == assignment.CareRequest.PatientId)
                .Select(p => p.UserId)
                .FirstAsync();

            await _notificationService.CreateAsync(
                patientUserId,
                "Care Completed",
                "Your care has been completed. Please rate your nurse.",
                "Patient");

            return new ServiceResult
            {
                Success = true,
                Message = "Assignment completed successfully."
            };
        }
        public async Task<List<WorkHistoryViewModel>> GetWorkHistoryAsync(string userId)
        {
            var nurse = await _context.Nurses
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (nurse == null)
                return new List<WorkHistoryViewModel>();

            return await _context.Assignments
                .Include(a => a.CareRequest)
                    .ThenInclude(c => c.Patient)
                        .ThenInclude(p => p.User)
                .Include(a => a.CareRequest)
                    .ThenInclude(c => c.Service)
                .Where(a =>
                    a.NurseId == nurse.Id &&
                    a.AssignmentStatus == "Completed")
                .OrderByDescending(a => a.CompletedAt)
                .Select(a => new WorkHistoryViewModel
                {
                    AssignmentId = a.Id,

                    PatientName =
                        a.CareRequest.Patient.User.FirstName + " " +
                        a.CareRequest.Patient.User.LastName,

                    ServiceName = a.CareRequest.Service.Name,

                    ShiftStart = a.ShiftStart,

                    ShiftEnd = a.ShiftEnd,

                    CompletedAt = a.CompletedAt,

                    Status = a.AssignmentStatus
                })
                .ToListAsync();
        }
    }
}