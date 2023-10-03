using Microsoft.AspNetCore.Mvc;

namespace webdemo.Controllers
{
    public class BaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
