using SqlSugar;
using webdemo.Models.Domain.System;
using webdemo.Models.Dto.Menu;

namespace webdemo.Services.Impl
{
    public class MenuService : IMenuService
    {
        private IMapper _mapper;
        private readonly SqlSugarScope _sqlSugarClient;
        private readonly IBaseRepository<Menu> _dal;
        public MenuService(IMapper mapper, SqlSugarScope sqlSugarClient, IBaseRepository<Menu> dal)
        {
            _mapper = mapper;
            _sqlSugarClient = sqlSugarClient;
            _dal = dal;
        }
        public DemoResult Create(Menu menu)
        {
            DemoResult result = new DemoResult();
            if (_dal.Insert(menu) > 0)
            {
                result.Success("添加成功");
            }
            else
            {
                result.Failed("添加失败");
            }
            return result;
        }

        public DemoResult Delete(long id)
        {
            DemoResult result = new DemoResult();
            var delete = _dal.QueryByClause(p => p.Id == id);
            delete.IsDel = true;
            if (_dal.Update(delete))
            {
                result.Success("删除成功");
            }
            else
            {
                result.Failed("删除失败");
            }
            return result;
        }

        public DemoResult Edit(Menu menu)
        {
            DemoResult result = new DemoResult();

            if (_dal.Update(menu))
            {
                result.Success("修改成功");
            }
            else
            {
                result.Failed("修改失败");
            }
            return result;
        }

        public Menu GetMenu(long id)
        {
            return _dal.QueryByClause(c => c.Id == id);
        }

        public IPagedList<Menu> GetMenuPage(MenuSearch search)
        {
            var where = PredicateBuilder.True<Menu>()
                         .WhereIf(true, p => p.IsDel == false);

            var result = _dal.QueryListByClause(where);

            return result.ToPagedList(search.PageIndex, search.PageSize);
        }
    }
}
