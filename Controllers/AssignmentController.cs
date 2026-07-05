using Microsoft.AspNetCore.Mvc;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels;

namespace NursingCarePlatform.Web.Controllers
{
    public class AssignmentController : Controller
    {
        private readonly IAssignmentService _assignmentService;

        public AssignmentController(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await _assignmentService.GetAssignmentDetailsAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new AssignmentViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AssignmentViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var assignmentId = await _assignmentService.CreateAssignmentAsync(model);

            return RedirectToAction(nameof(Details), new { id = assignmentId });
        }
    }
}