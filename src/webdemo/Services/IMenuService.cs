using webdemo.Models.Dto.Menu;
using webdemo.Models.Vo.Menu;

namespace webdemo.Services
{
    public interface IMenuService
    {
        Menu GetMenu(long id);

        List<Menu> GetMenuList(MenuSearch search);

        List<MenuTreeVo> GetMenuTree();

        DemoResult Create(Menu menu);

        DemoResult Edit(Menu menu);

        DemoResult Delete(long id);
    }
}
