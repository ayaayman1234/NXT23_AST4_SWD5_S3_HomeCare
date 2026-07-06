using Microsoft.EntityFrameworkCore;
using NursingCarePlatform.Web.Data;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Models.Responses;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels.NursingNote;

namespace NursingCarePlatform.Web.Services.Implementations
{
    public class NursingNoteService : INursingNoteService
    {
        private readonly NursingDbContext _context;

        public NursingNoteService(NursingDbContext context)
        {
            _context = context;
        }

        // ===========================
        // Get Assignment
        // ===========================

        public async Task<CreateNursingNoteViewModel?> GetAssignmentAsync(int assignmentId)
        {
            var assignment = await _context.Assignments
                .FirstOrDefaultAsync(a => a.Id == assignmentId);

            if (assignment == null)
                return null;

            return new CreateNursingNoteViewModel
            {
                AssignmentId = assignment.Id
            };
        }

        // ===========================
        // Check Existing Note
        // ===========================

        public async Task<bool> HasNoteAsync(int assignmentId)
        {
            return await _context.NursingNotes
                .AnyAsync(n => n.AssignmentId == assignmentId);
        }

        // ===========================
        // Create Nursing Note
        // ===========================

        public async Task<ServiceResult> CreateAsync(CreateNursingNoteViewModel model)
        {
            bool exists = await HasNoteAsync(model.AssignmentId);

            if (exists)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "This assignment already has a nursing note."
                };
            }

            var note = new NursingNote
            {
                AssignmentId = model.AssignmentId,

                BloodPressure = model.BloodPressure,

                GlucoseLevel = model.GlucoseLevel,

                Temperature = model.Temperature,

                Notes = model.Notes,

                UploadedAt = DateTime.Now
            };

            _context.NursingNotes.Add(note);

            await _context.SaveChangesAsync();

            return new ServiceResult
            {
                Success = true,
                Message = "Nursing note added successfully."
            };
        }
    }
}