using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels.Payment;

namespace NursingCarePlatform.Web.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        
        public async Task<IActionResult> Index()
        {
            var payments = await _paymentService.GetAllPaymentsAsync();
            return View(payments);
        }

        public async Task<IActionResult> History()
        {
            var history = await _paymentService.GetPaymentHistoryAsync();
            return View(history);
        }

        
        public async Task<IActionResult> Details(int id)
        {
            var payment = await _paymentService.GetPaymentDetailsAsync(id);

            if (payment == null)
                return NotFound();

            return View(payment);
        }

        [HttpGet]
        public async Task<IActionResult> Pay(int careRequestId)
        {
            var model =
                await _paymentService.GetPaymentForRequestAsync(careRequestId);

            if (model == null)
                return NotFound();

            return View(model);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pay(CreatePaymentViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result =
                await _paymentService.CreatePaymentAsync(model);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(
                "RequestDetails",
                "Patient",
                new { id = model.CareRequestId });
        }
    }
}