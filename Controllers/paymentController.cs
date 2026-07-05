using Microsoft.AspNetCore.Mvc;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels;

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

        [HttpGet]
        public IActionResult Create()
        {
            return View(new PaymentViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var paymentId = await _paymentService.CreatePaymentAsync(model);

            return RedirectToAction(nameof(Details), new { id = paymentId });
        }
    }
}