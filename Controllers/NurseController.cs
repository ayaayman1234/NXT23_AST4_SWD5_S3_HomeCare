using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NursingCarePlatform.Web.Models;
using NursingCarePlatform.Web.Services.Interfaces;
using NursingCarePlatform.Web.ViewModels.Nurse;

namespace NursingCarePlatform.Web.Controllers
{
    [Authorize(Roles = "Nurse")]
    public class NurseController : Controller
    {
        private readonly INurseService _nurseService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISubscriptionService _subscriptionService;

        public NurseController(
            INurseService nurseService,
            UserManager<ApplicationUser> userManager,
            ISubscriptionService subscriptionService)
        {
            _nurseService = nurseService;
            _userManager = userManager;
            _subscriptionService = subscriptionService;
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
            if (user != null)
            {
                var nurse = await _nurseService.GetNurseByUserIdAsync(user.Id);
                if (nurse != null)
                {
                    var subs = await _subscriptionService.GetNurseSubscriptionsAsync(nurse.Id);
                    var activeSub = subs
                        .Where(s => s.Status == "Active" && s.EndDate >= DateTime.Now)
                        .OrderByDescending(s => s.StartDate)
                        .FirstOrDefault();

                    // Fallback for old nurses created before the subscription system
                    if (activeSub == null)
                    {
                        var plans = await _subscriptionService.GetAllPlansAsync();
                        var freePlan = plans.FirstOrDefault(p => p.MonthlyFee == 0);
                        if (freePlan != null)
                        {
                            await _subscriptionService.AssignPlanToNurseAsync(nurse.Id, freePlan.Id, DateTime.Now, DateTime.Now.AddYears(10));
                            
                            // Reload after assignment
                            subs = await _subscriptionService.GetNurseSubscriptionsAsync(nurse.Id);
                            activeSub = subs
                                .Where(s => s.Status == "Active" && s.EndDate >= DateTime.Now)
                                .OrderByDescending(s => s.StartDate)
                                .FirstOrDefault();
                        }
                    }

                    ViewBag.ActiveSubscription = activeSub;
                }
            }
            return View();
        }

        // =====================================
        // Profile
        // =====================================

        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var profile = await _nurseService.GetProfileAsync(user.Id);

            if (profile == null)
                return NotFound();

            return View(profile);
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

            var nurse = await _nurseService.GetNurseByUserIdAsync(user.Id);

            if (nurse == null)
                return NotFound();

            var model = new EditNurseViewModel
            {
                Id = nurse.Id,
                UserId = nurse.UserId,

                FirstName = nurse.User.FirstName,
                LastName = nurse.User.LastName,

                Email = nurse.User.Email ?? "",
                PhoneNumber = nurse.User.PhoneNumber ?? "",

                Address = nurse.User.Address,
                City = nurse.User.City,
                Governorate = nurse.User.Governorate,

                Gender = nurse.User.Gender,
                Age = nurse.User.Age,

                YearsExperience = nurse.YearsExperience,
                Specialization = nurse.Specialization,
               
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditNurseViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var result = await _nurseService.UpdateProfileAsync(user.Id, model);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(Profile));
        }
        // =====================================
        // Availability
        // =====================================

        public async Task<IActionResult> Availability()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var availability = await _nurseService.GetAvailabilityAsync(user.Id);

            return View(availability);
        }


        // =====================================
        // Add Availability
        // =====================================

        [HttpGet]
        public IActionResult AddAvailability()
        {
            return View(new AvailabilityViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddAvailability(AvailabilityViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var result = await _nurseService.AddAvailabilityAsync(user.Id, model);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(Availability));
        }

        // =====================================
        // Delete Availability
        // =====================================

        public async Task<IActionResult> DeleteAvailability(int id)
        {
            var result = await _nurseService.DeleteAvailabilityAsync(id);

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(Availability));
        }

        // =====================================
        // Documents
        // =====================================

        public async Task<IActionResult> Documents()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var documents = await _nurseService.GetDocumentsAsync(user.Id);

            return View(documents);
        }

        [HttpGet]
        public IActionResult UploadDocument()
        {
            return View(new DocumentViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadDocument(DocumentViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var result = await _nurseService.UploadDocumentAsync(user.Id, model);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(Documents));
        }

        public async Task<IActionResult> DeleteDocument(int id)
        {
            var result = await _nurseService.DeleteDocumentAsync(id);

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(Documents));
        }
        // =====================================
        // Services
        // =====================================

        public async Task<IActionResult> Services()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var services = await _nurseService.GetServicesAsync(user.Id);

            var model = services.Select(x => new NurseServiceViewModel
            {
                ServiceId = x.ServiceId,
                ServiceName = x.Service.Name,
                CategoryName = x.Service.Category.Name,
                Price = x.Price
            }).ToList();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddService()
        {
            ViewBag.Categories = await _nurseService.GetCategoriesAsync();

            ViewBag.Services = new List<NursingService>();

            return View(new NurseServiceViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddService(NurseServiceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _nurseService.GetCategoriesAsync();
                ViewBag.Services = await _nurseService.GetServicesByCategoryAsync(model.CategoryId);

                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var result = await _nurseService.AddServiceAsync(user.Id, model);

            if (!result.Success)
            {
                ViewBag.Categories = await _nurseService.GetCategoriesAsync();
                ViewBag.Services = await _nurseService.GetServicesByCategoryAsync(model.CategoryId);

                ModelState.AddModelError("", result.Message);

                return View(model);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(Services));
        }
        [HttpGet]
        public async Task<JsonResult> GetServicesByCategory(int categoryId)
        {
            var services = await _nurseService.GetServicesByCategoryAsync(categoryId);

            return Json(services.Select(s => new
            {
                id = s.Id,
                name = s.Name
            }));
        }
        public async Task<IActionResult> DeleteService(int serviceId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var result = await _nurseService.DeleteServiceAsync(user.Id, serviceId);

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(Services));
        }
        // =====================================
        // Available Requests
        // =====================================

        public async Task<IActionResult> AvailableRequests()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var nurse = await _nurseService.GetNurseByUserIdAsync(user.Id);
            ViewBag.IsVerified = nurse?.IsVerified ?? false;

            var requests = await _nurseService.GetAvailableRequestsAsync(user.Id);

            return View(requests);
        }
    }
}