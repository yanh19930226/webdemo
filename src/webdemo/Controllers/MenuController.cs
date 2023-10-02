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

        public IActionResult GetMenuList(MenuSearch search)
        {
            var result = _menuService.GetMenuList(search);
            return Json(result);
        }

        public IActionResult Create(long parentId)
        {
            Menu menu = new Menu();
            menu.ParentId = parentId;
            return PartialView(menu);
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
        public IActionResult DoEdit(MenuEditDto dto)
        {
            return Ok(_menuService.Edit(_mapper.Map<Menu>(dto)));
        }

        public IActionResult Delete(long id)
        {
            return Ok(_menuService.Delete(id));
        }
    }
}
