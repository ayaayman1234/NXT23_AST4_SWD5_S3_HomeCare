using Microsoft.AspNetCore.Mvc;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels.Payment;

namespace NursingCarePlatform.Web.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await _paymentService.GetPaymentDetailsAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        // ── Pay (GET) ─────────────────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> Pay(int careRequestId)
        {
            var model = await _paymentService.GetPaymentForRequestAsync(careRequestId);

            if (model == null)
            {
                TempData["Error"] = "Payment details not found or already paid.";
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        // ── Pay (POST) ────────────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pay(CreatePaymentViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _paymentService.CreatePaymentAsync(model);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }

            return RedirectToAction(nameof(Details), new { id = result.DataId });
        }

        // ── Create (GET) ──────────────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> Create(int careRequestId)
        {
            var model = await _paymentService.GetPaymentForRequestAsync(careRequestId);

            if (model == null)
            {
                TempData["Error"] = "Payment details not found or already paid.";
                return RedirectToAction("Index", "Home");
            }

            return View("Payment", model);
        }

        // ── Create (POST) ─────────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePaymentViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Payment", model);

            var result = await _paymentService.CreatePaymentAsync(model);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View("Payment", model);
            }

            return RedirectToAction(nameof(Details), new { id = result.DataId });
        }
    }
}