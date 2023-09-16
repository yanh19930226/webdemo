using webdemo.Models.Vo.Menu;

namespace webdemo.Views.Shared.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        public MenuViewComponent()
        {

        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var menus = new List<MenuVo>() {
               new MenuVo()
               {
                   Id=1,
                   ParentId=0,
                   Type=1,
                   Name = "基础管理",
                   Url = "",
               },
                new MenuVo()
               {
                   Id=2,
                   ParentId=1,
                   Type=2,
                   Name = "用户管理",
                   Url = "/User",
               },
                 new MenuVo()
               {
                   Id=3,
                   ParentId=0,
                   Type=1,
                   Name = "日志管理",
                   Url = "",
               },
               new MenuVo()
               {
                   Id=4,
                   ParentId=3,
                   Type=2,
                   Name = "登入日志",
                   Url = "/User",
               },
               new MenuVo()
               {
                   Id=5,
                   ParentId=3,
                   Type=2,
                   Name = "操作日志",
                   Url = "/User",
               }
            };
            return View(menus);
        }
    }
}
