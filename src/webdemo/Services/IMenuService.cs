using webdemo.Models.Domain.System;
using webdemo.Models.Dto.Menu;

namespace webdemo.Services
{
    public interface IMenuService
    {
        Menu GetMenu(long id);

        IPagedList<Menu> GetMenuPage(MenuSearch search);

        DemoResult Create(Menu menu);

        DemoResult Edit(Menu menu);

        DemoResult Delete(long id);
    }
}
