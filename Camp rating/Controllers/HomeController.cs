using Microsoft.AspNetCore.Mvc;

namespace Camp_rating.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
