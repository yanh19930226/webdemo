using webdemo.Models.Dto.Role;

namespace webdemo.Controllers.System
{
    public class RoleController : Controller
    {
        private IMapper _mapper;
        private IRoleService _roleService;
        public RoleController(IMapper mapper, IRoleService roleService)
        {
            _mapper = mapper;
            _roleService = roleService;
        }

        public IActionResult Index(RoleSearch search)
        {
            return View(search);
        }

        public IActionResult DoList(RoleSearch search)
        {
            var result = _roleService.GetRolePage(search);
            return PartialView(result);
        }

        public IActionResult Create()
        {
            Role menu = new Role();
            return PartialView(menu);
        }

        [HttpPost]
        public IActionResult DoCreate(Role dto)
        {
            return Ok(_roleService.Create(dto));
        }

        public IActionResult Edit(long id)
        {
            var edit = _roleService.GetRole(id);
            return View(edit);
        }
        [HttpPost]
        public IActionResult DoEdit(Role dto)
        {
            return Ok(_roleService.Edit(dto));
        }

        public IActionResult Delete(long id)
        {
            return Ok(_roleService.Delete(id));
        }
    }
}
