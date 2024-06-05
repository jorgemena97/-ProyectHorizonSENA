using Microsoft.AspNetCore.Mvc;

namespace PorjectHorizonSENA.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Chat()
        {
            return View();
        }
    }
}
