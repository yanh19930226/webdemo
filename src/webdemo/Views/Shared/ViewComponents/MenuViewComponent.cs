using webdemo.Models.Dto.Menu;

namespace webdemo.Views.Shared.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private IMenuService _menuService;
        public MenuViewComponent(IMenuService menuService)
        {
            _menuService= menuService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var menus = _menuService.GetMenuList(new MenuSearch()).Where(p=>p.IsShow=true).ToList();
            return View(menus);
        }
    }
}
