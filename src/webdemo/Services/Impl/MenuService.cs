using webdemo.Models.Dto.Menu;
using webdemo.Models.Vo.Menu;

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

        /// <summary>
        /// 获取子节点
        /// </summary>
        /// <param name="list"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        private List<MenuTreeVo> GetChildren(List<Menu> list, long? id)
        {
            List<MenuTreeVo> nodeList = new List<MenuTreeVo>();
            var children = list.Where(q => q.ParentId == id);
            foreach (var item in children)
            {
                MenuTreeVo node = new MenuTreeVo();
                node.Menu = item;
                node.Children = GetChildren(list, node.Menu.Id);
                nodeList.Add(node);
            }
            return nodeList;
        }

        public Menu GetMenu(long id)
        {
            return _dal.QueryByClause(c => c.Id == id);
        }

        public List<Menu> GetMenuList(MenuSearch search)
        {
            var where = PredicateBuilder.True<Menu>()
                         .WhereIf(true, p => p.IsDel == false)
                         .WhereIf(!string.IsNullOrEmpty(search.Keyword), p => p.MenuName.Contains(search.Keyword));
            return _dal.QueryListByClause(where).ToList();
        }

        public List<MenuTreeVo> GetMenuTree()
        {
            var result = _dal.QueryListByClause(p => p.IsDel == false).ToList();
            return GetChildren(result, 0);
        }

        public DemoResult Create(Menu menu)
        {
            DemoResult result = new DemoResult();
            if (_dal.Insert(menu) > 0)
            {
                result.Success();
            }
            else
            {
                result.Failed();
            }
            return result;
        }

        public DemoResult Edit(Menu menu)
        {
            DemoResult result = new DemoResult();

            if (_dal.Update(menu))
            {
                result.Success();
            }
            else
            {
                result.Failed();
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
                result.Success();
            }
            else
            {
                result.Failed();
            }
            return result;
        }
    }
}
