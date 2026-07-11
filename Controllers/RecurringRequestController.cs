using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels.RecurringRequest;

namespace NursingCarePlatform.Web.Controllers
{
    [Authorize]
    public class RecurringRequestController : Controller
    {
        private readonly IRecurringRequestService _recurringRequestService;

        public RecurringRequestController(IRecurringRequestService recurringRequestService)
        {
            _recurringRequestService = recurringRequestService;
        }

        // ==========================
        // All Recurring Requests
        // ==========================

        public async Task<IActionResult> Index()
        {
            var requests = await _recurringRequestService.GetAllRecurringRequestsAsync();

            return View(requests);
        }

        // ==========================
        // Details
        // ==========================

        public async Task<IActionResult> Details(int id)
        {
            var request =
                await _recurringRequestService.GetRecurringRequestDetailsAsync(id);

            if (request == null)
                return NotFound();

            return View(request);
        }

        // ==========================
        // Create
        // ==========================

        [HttpGet]
        public IActionResult Create(int careRequestId)
        {
            return View(new CreateRecurringRequestViewModel
            {
                CareRequestId = careRequestId,
                StartDate = DateTime.Today
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRecurringRequestViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result =
                await _recurringRequestService.CreateRecurringRequestAsync(model);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(Index));
        }
    }
}