namespace webdemo.Controllers
{
    public class HomeController : Controller
    {
        private DemoDbContext _dbContext;
        private IHttpContextAccessor _httpcontext;
        public HomeController(DemoDbContext dbContext, IHttpContextAccessor httpcontext)
        {
            _dbContext = dbContext;
            _httpcontext= httpcontext;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var IsAuthenticated = _httpcontext.HttpContext.User?.Identity?.IsAuthenticated ?? false;
            if (IsAuthenticated)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"当前登录用户：{_httpcontext.HttpContext.User.Identity.Name}");
                sb.Append($"验证类型：{_httpcontext.HttpContext.User.Identity.AuthenticationType}");
                foreach (var item in _httpcontext.HttpContext.User.Claims)
                {
                    sb.Append($"{item.Type}-{item.Value}");
                }
                ViewBag.UserMessage = sb.ToString();
            }
            ViewBag.IsAuthenticated = IsAuthenticated;
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> DoLogin(LoginDto dto, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var user = await _dbContext.User.FirstOrDefaultAsync(p => p.UserName == dto.UserName && p.Password == dto.Password);

                if (user != null)
                {
                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                    var claimsIdentity = new ClaimsIdentity(claims, "webdemo");
                    await HttpContext.SignInAsync("webdemo", new ClaimsPrincipal(claimsIdentity));
                    return RedirectToLocal(returnUrl);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync("webdemo");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Pwd()
        {
            return View();
        }
        
        public IActionResult Deny()
        {
            return View();
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
