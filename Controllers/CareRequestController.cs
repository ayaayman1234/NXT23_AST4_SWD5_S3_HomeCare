using Microsoft.AspNetCore.Mvc;
using NursingCarePlatform.Web.Services;
using NursingCarePlatform.Web.ViewModels;
using NursingCarePlatform.Web.ViewModels.CareRequest;

namespace NursingCarePlatform.Web.Controllers
{
    public class CareRequestController : Controller
    {
        private readonly ICareRequestService _careRequestService;

        public CareRequestController(ICareRequestService careRequestService)
        {
            _careRequestService = careRequestService;
        }

        // =========================
        // My Requests
        // =========================
        public async Task<IActionResult> MyRequests()
        {
            // Replace this with the logged-in patient id
            int patientId = 1;

            var requests = await _careRequestService.GetPatientRequestsAsync(patientId);

            return View(requests);
        }

        // =========================
        // Details
        // =========================
        public async Task<IActionResult> Details(int id)
        {
            var request = await _careRequestService.GetRequestDetailsAsync(id);

            if (request == null)
                return NotFound();

            return View(request);
        }

        // =========================
        // Create
        // =========================
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCareRequestViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Replace with logged-in patient id
            int patientId = 1;

            await _careRequestService.CreateRequestAsync(model, patientId);

            TempData["Success"] = "Care Request created successfully.";

            return RedirectToAction(nameof(MyRequests));
        }

        // =========================
        // Edit
        // =========================
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _careRequestService.GetRequestForEditAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditCareRequestViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _careRequestService.EditRequestAsync(model);

            TempData["Success"] = "Care Request updated successfully.";

            return RedirectToAction(nameof(MyRequests));
        }

        // =========================
        // Delete
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _careRequestService.DeleteRequestAsync(id);

            TempData["Success"] = "Care Request deleted successfully.";

            return RedirectToAction(nameof(MyRequests));
        }
    }
}