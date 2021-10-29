using Microsoft.AspNetCore.Mvc;

namespace WEB.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}