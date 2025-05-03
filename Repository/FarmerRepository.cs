using Microsoft.AspNetCore.Mvc;

namespace ST10263027_PROG7311_POE.Repository
{
    public class FarmerRepository : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
