using Microsoft.AspNetCore.Mvc;

namespace webdemo.Controllers
{
    public class ArticleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
