using Microsoft.AspNetCore.Mvc;

namespace NursingCarePlatform.Web.Controllers
{
    public class AdminAccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
