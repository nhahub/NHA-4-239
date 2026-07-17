using Microsoft.AspNetCore.Mvc;

namespace HouraPL.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index() => View();
    }
}

