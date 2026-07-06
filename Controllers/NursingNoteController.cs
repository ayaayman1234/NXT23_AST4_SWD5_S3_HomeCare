using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels.NursingNote;

namespace NursingCarePlatform.Web.Controllers
{
    [Authorize(Roles = "Nurse")]
    public class NursingNoteController : Controller
    {
        private readonly INursingNoteService _nursingNoteService;

        public NursingNoteController(INursingNoteService nursingNoteService)
        {
            _nursingNoteService = nursingNoteService;
        }

        // ===========================
        // GET
        // ===========================

        [HttpGet]
        public async Task<IActionResult> Create(int assignmentId)
        {
            var exists = await _nursingNoteService.HasNoteAsync(assignmentId);

            if (exists)
            {
                TempData["Error"] = "This assignment already has a nursing note.";
                return RedirectToAction("MyAssignments", "Assignment");
            }

            var model = await _nursingNoteService.GetAssignmentAsync(assignmentId);

            if (model == null)
                return NotFound();

            return View(model);
        }

        // ===========================
        // POST
        // ===========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateNursingNoteViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _nursingNoteService.CreateAsync(model);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction("MyAssignments", "Assignment");
        }
    }
}