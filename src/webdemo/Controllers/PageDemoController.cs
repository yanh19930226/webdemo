namespace webdemo.Controllers
{
    public class PageDemoController : Controller
    {
        private IUserService _userService;
        public PageDemoController(IUserService userService)
        {
            _userService= userService;
        }

        public IActionResult Index(PageDemoSearch demoSearch)
        {
            demoSearch.PageIndex = 1;
            demoSearch.PageSize = 10;
            return View(demoSearch);
        }

        public IActionResult DoList(PageDemoSearch demoSearch)
        {
            var model = _userService.GetPagedOrders(demoSearch.PageIndex, demoSearch.PageSize, demoSearch.CompanyName);
            return PartialView(model);
        }

        public IActionResult PartialIndex(string companyName, int PageIndex = 1)
        {
            var model = _userService.GetPagedOrders(PageIndex, 5, companyName);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_PagedData", model);
            }
            return View(model);
        }
    }
}
