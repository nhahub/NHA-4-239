using System;
using Microsoft.AspNetCore.Mvc;

namespace HouraPL.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            if (!Guid.TryParse(HttpContext.Session.GetString("UserId"), out _))
                return RedirectToAction("Login", "Account");

            return View();
        }
    }
}
