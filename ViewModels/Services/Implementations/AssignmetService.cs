using Microsoft.EntityFrameworkCore;
using NursingCarePlatform.Web.Data;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels;

namespace NursingCarePlatform.Web.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly ApplicationDbContext _context;

        public AssignmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAssignmentAsync(AssignmentViewModel model)
        {
            var careRequestExists = await _context.CareRequests
                .AnyAsync(cr => cr.Id == model.CareRequestId);

            if (!careRequestExists)
                throw new InvalidOperationException("Invalid CareRequestId.");

            // Multiple nurses can be assigned to the same CareRequest independently.
            var assignment = new Assignment
            {
                CareRequestId = model.CareRequestId,
                NurseId = model.NurseId,
                ShiftStart = model.ShiftStart,
                ShiftEnd = model.ShiftEnd,
                AssignmentStatus = model.AssignmentStatus
            };

            _context.Assignments.Add(assignment);
            await _context.SaveChangesAsync();

            return assignment.Id;
        }

        public async Task<AssignmentDetailsViewModel?> GetAssignmentDetailsAsync(int id)
        {
            var assignment = await _context.Assignments
                .Include(a => a.CareRequest)
                    .ThenInclude(cr => cr.Patient)
                .Include(a => a.Nurse)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (assignment == null)
                return null;

            return new AssignmentDetailsViewModel
            {
                Id = assignment.Id,
                CareRequestId = assignment.CareRequestId,
                PatientName = assignment.CareRequest.Patient.FullName,
                NurseName = assignment.Nurse.FullName,
                ShiftStart = assignment.ShiftStart,
                ShiftEnd = assignment.ShiftEnd,
                AssignmentStatus = assignment.AssignmentStatus
            };
        }
    }
}