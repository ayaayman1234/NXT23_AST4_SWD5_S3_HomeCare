using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels.MedicalChecklist;

namespace NursingCarePlatform.Web.Controllers
{
    [Authorize(Roles = "Nurse")]
    public class MedicalChecklistController : Controller
    {
        private readonly IMedicalChecklistService _medicalChecklistService;

        public MedicalChecklistController(IMedicalChecklistService medicalChecklistService)
        {
            _medicalChecklistService = medicalChecklistService;
        }

        // ===========================
        // GET
        // ===========================

        [HttpGet]
        public async Task<IActionResult> Create(int requestId)
        {
            if (await _medicalChecklistService.ExistsAsync(requestId))
            {
                TempData["Info"] = "Medical Checklist already exists.";

                return RedirectToAction("MyAssignments", "Assignment");
            }

            var model = await _medicalChecklistService.GetRequestAsync(requestId);

            if (model == null)
                return NotFound();

            return View(model);
        }

        // ===========================
        // POST
        // ===========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateMedicalChecklistViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _medicalChecklistService.CreateAsync(model);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction("MyAssignments", "Assignment");
        }
        public async Task<IActionResult> Details(int requestId)
        {
            var checklist = await _medicalChecklistService.GetByRequestIdAsync(requestId);

            if (checklist == null)
                return NotFound();

            return View(checklist);
        }
    }
}