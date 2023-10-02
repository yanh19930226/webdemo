using webdemo.Models.Dto.Menu;

namespace webdemo.Controllers
{
    public class MenuController : Controller
    {
        private IMapper _mapper;
        private IMenuService _menuService;
        public MenuController(IMapper mapper, IMenuService menuService)
        {
            _mapper = mapper;
            _menuService = menuService;
        }

        public IActionResult Index(MenuSearch search)
        {
            return View(search);
        }


        public IActionResult GetMenuPage(MenuSearch search)
        {
            var result = _menuService.GetMenuPage(search);
            return Json(result);
        }

        public IActionResult Create(Menu dto)
        {
            return PartialView(dto);
        }

        [HttpPost]
        public IActionResult DoCreate(Menu dto)
        {
            return Ok(_menuService.Create(dto));
        }

        public IActionResult Edit(long id)
        {
            var edit = _menuService.GetMenu(id);
            return View(edit);
        }
        [HttpPost]
        public IActionResult DoEdit(Menu dto)
        {
            return Ok(_menuService.Edit(dto));
        }

        public IActionResult Delete(long id)
        {
            return Ok(_menuService.Delete(id));
        }
    }
}
