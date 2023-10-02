using Microsoft.AspNetCore.Mvc;

namespace webdemo.Controllers.System
{
    public class OrganizationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
