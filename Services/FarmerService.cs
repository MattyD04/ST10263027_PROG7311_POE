using Microsoft.AspNetCore.Mvc;

namespace ST10263027_PROG7311_POE.Services
{
    public class FarmerService : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
