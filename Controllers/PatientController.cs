using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NursingCarePlatform.Web.Data;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels.CareRequest;
using NursingCarePlatform.Web.ViewModels.Patient;

namespace NursingCarePlatform.Web.Controllers
{
    [Authorize(Roles = "Patient")]
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly NursingDbContext _context;
        private readonly IOfferService _offerService;

        public PatientController(
    IPatientService patientService,
    UserManager<ApplicationUser> userManager,
    NursingDbContext context,
    IOfferService offerService)
        {
            _patientService = patientService;
            _userManager = userManager;
            _context = context;
            _offerService = offerService;
        }

        public override async Task OnActionExecutionAsync(
            Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context,
            Microsoft.AspNetCore.Mvc.Filters.ActionExecutionDelegate next)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null && user.AccountStatus == "Blocked")
            {
                var action = context.RouteData.Values["action"]?.ToString();
                if (action != "Blocked")
                {
                    context.Result = RedirectToAction("Blocked");
                    return;
                }
            }
            await base.OnActionExecutionAsync(context, next);
        }

        [AllowAnonymous]
        public IActionResult Blocked()
        {
            return View();
        }

        // =====================================
        // Dashboard
        // =====================================

        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var requests = await _patientService.GetMyRequestsAsync(user.Id);

            return View(requests);
        }

        // =====================================
        // Profile
        // =====================================

        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var patient = await _patientService.GetPatientByUserIdAsync(user.Id);

            if (patient == null)
                return NotFound();

            return View(patient);
        }

        // =====================================
        // Edit Profile
        // =====================================

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var patient = await _patientService.GetPatientByUserIdAsync(user.Id);

            if (patient == null)
                return NotFound();

            var model = new EditPatientViewModel
            {
                Id = patient.Id,
                UserId = patient.UserId,

                FirstName = patient.User.FirstName,
                LastName = patient.User.LastName,
                Email = patient.User.Email ?? string.Empty,
                PhoneNumber = patient.User.PhoneNumber,
                Gender = patient.User.Gender,
                Age = patient.User.Age,
                Address = patient.User.Address,
                City = patient.User.City,
                Governorate = patient.User.Governorate,
                BloodType = patient.BloodType,

                MedicalHistory = patient.MedicalHistory
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditPatientViewModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var item in ModelState)
                {
                    foreach (var error in item.Value.Errors)
                    {
                        Console.WriteLine($"{item.Key} ==> {error.ErrorMessage}");
                    }
                }

                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var result = await _patientService.UpdateProfileAsync(user.Id, model);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(Profile));
        }

        // =====================================
        // My Requests
        // =====================================

        public async Task<IActionResult> MyRequests()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var requests = await _patientService.GetMyRequestsAsync(user.Id);

            ViewBag.TotalRequests = requests.Count();

            ViewBag.PendingRequests = requests.Count(x =>
                x.RequestStatus == "Pending");

            ViewBag.CompletedRequests = requests.Count(x =>
                x.RequestStatus == "Completed");

            return View(requests);
        }

        // =====================================
        // Request Details
        // =====================================

        public async Task<IActionResult> RequestDetails(int id)
        {
            var request = await _patientService.GetRequestByIdAsync(id);

            if (request == null)
                return NotFound();

            var offers = await _offerService.GetOffersForRequestAsync(id);

            Console.WriteLine("Offers Count = " + offers.Count);

            foreach (var item in offers)
            {
                Console.WriteLine(item.Id + " " + item.CareRequestId);
            }

            ViewBag.Offers = offers;
            ViewBag.IsPaid = await _context.Payments
    .AnyAsync(p => p.CareRequestId == id);

            return View(request);
        }

        // =====================================
        // Create Care Request
        // =====================================

        [HttpGet]
        public async Task<IActionResult> CreateCareRequest()
        {
            var model = new CreateCareRequestViewModel
            {
                PreferredDate = DateTime.Today.AddDays(1),
                Services = await _patientService.GetServicesAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCareRequest(CreateCareRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"Field: {error.Key}");

                    foreach (var item in error.Value.Errors)
                    {
                        Console.WriteLine($"Error: {item.ErrorMessage}");
                    }
                }

                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var result = await _patientService.CreateRequestAsync(user.Id, model);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(MyRequests));
        }

        // =====================================
        // Edit Request
        // =====================================

        [HttpGet]
        public async Task<IActionResult> EditRequest(int id)
        {
            var request = await _patientService.GetRequestByIdAsync(id);

            if (request == null)
                return NotFound();

            var model = new EditCareRequestViewModel
            {
                Id = request.Id,
                PatientId = request.PatientId,
                ServiceId = request.ServiceId,
                Address = request.Address,
                Description = request.Description,
                PreferredDate = request.PreferredDate,
                BudgetMin = request.BudgetMin,
                BudgetMax = request.BudgetMax,

                Services = await _context.NursingServices
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name
                    })
                    .ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRequest(EditCareRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var item in ModelState)
                {
                    foreach (var error in item.Value.Errors)
                    {
                        Console.WriteLine($"{item.Key} ==> {error.ErrorMessage}");
                    }
                }

                model.Services = await _context.NursingServices
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name
                    })
                    .ToListAsync();

                return View(model);
            }

            Console.WriteLine("Model is Valid");

            var result = await _patientService.UpdateRequestAsync(model);

            Console.WriteLine($"Success = {result.Success}");
            Console.WriteLine($"Message = {result.Message}");

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);

                model.Services = await _context.NursingServices
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name
                    })
                    .ToListAsync();

                return View(model);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(MyRequests));
        }

        // =====================================
        // Delete Request
        // =====================================

        public async Task<IActionResult> DeleteRequest(int id)
        {
            await _patientService.DeleteRequestAsync(id);

            return RedirectToAction(nameof(MyRequests));
        }

        // =====================================
        // Cancel Request
        // =====================================

        public async Task<IActionResult> CancelRequest(int id)
        {
            await _patientService.CancelRequestAsync(id);

            return RedirectToAction(nameof(MyRequests));
        }
        // =====================================
        // Rate Nurse
        // =====================================

        public IActionResult RateNurse(int requestId, int nurseId)
        {
            return RedirectToAction(
                "Create",
                "Rating",
                new
                {
                    careRequestId = requestId,
                    nurseId = nurseId
                });
        }
    }
}