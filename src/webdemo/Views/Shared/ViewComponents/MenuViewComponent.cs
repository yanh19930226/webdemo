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
                   MenuType=1,
                   MenuName = "基础管理",
                   Path = "",
               },
               new MenuVo()
               {
                   Id=2,
                   ParentId=1,
                   MenuType=2,
                   MenuName = "用户管理",
                   Path = "/User",
               },

               new MenuVo()
               {
                   Id=11,
                   ParentId=1,
                   MenuType=2,
                   MenuName = "菜单管理",
                   Path = "/Menu",
               },
               new MenuVo()
               {
                   Id=5,
                   ParentId=1,
                   MenuType=2,
                   MenuName = "分页测试",
                   Path = "/PageDemo",
               },
               new MenuVo()
               {
                   Id=6,
                   ParentId=1,
                   MenuType=2,
                   MenuName = "Partial分页测试",
                   Path = "/PageDemo/PartialIndex",
               },

               new MenuVo()
               {
                   Id=7,
                   ParentId=0,
                   MenuType=1,
                   MenuName = "内容管理",
                   Path = "",
               },


               new MenuVo()
               {
                   Id=8,
                   ParentId=7,
                   MenuType=2,
                   MenuName = "分类管理",
                   Path = "/Category",
               },


               new MenuVo()
               {
                   Id=3,
                   ParentId=0,
                   MenuType=1,
                   MenuName = "日志管理",
                   Path = "",
               },
               new MenuVo()
               {
                   Id=4,
                   ParentId=3,
                   MenuType=2,
                   MenuName = "登入日志",
                   Path = "/User",
               },
               new MenuVo()
               {
                   Id=5,
                   ParentId=3,
                   MenuType=2,
                   MenuName = "操作日志",
                   Path = "/User",
               }
            };
            return View(menus);
        }
    }
}
