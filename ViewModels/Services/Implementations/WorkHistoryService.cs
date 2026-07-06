using Microsoft.EntityFrameworkCore;
using NursingCarePlatform.Web.Data;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Services.Interfaces;

namespace NursingCarePlatform.Web.Services.Implementations
{
    public class WorkHistoryService : IWorkHistoryService
    {
        private readonly NursingDbContext _context;

        public WorkHistoryService(NursingDbContext context)
        {
            _context = context;
        }

        public async Task<List<WorkHistory>> GetHistoryAsync(string userId)
        {
            var nurse = await _context.Nurses
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (nurse == null)
                return new List<WorkHistory>();

            return await _context.WorkHistories
                .Include(x => x.Patient)
                    .ThenInclude(p => p.User)
                .Include(x => x.Service)
                .Where(x => x.NurseId == nurse.Id)
                .OrderByDescending(x => x.CompletedAt)
                .ToListAsync();
        }
    }
}