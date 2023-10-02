using webdemo.Models.Dto.Role;

namespace webdemo.Services.Impl
{
    public class RoleService : IRoleService
    {
        private IMapper _mapper;
        private readonly SqlSugarScope _sqlSugarClient;
        private readonly IBaseRepository<Role> _dal;
        public RoleService(IMapper mapper, SqlSugarScope sqlSugarClient, IBaseRepository<Role> dal)
        {
            _mapper = mapper;
            _sqlSugarClient = sqlSugarClient;
            _dal = dal;
        }
        public DemoResult Create(Role role)
        {
            DemoResult result = new DemoResult();
            if (_dal.Insert(role) > 0)
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

        public DemoResult Edit(Role role)
        {
            DemoResult result = new DemoResult();

            if (_dal.Update(role))
            {
                result.Success();
            }
            else
            {
                result.Failed();
            }
            return result;
        }

        public Role GetRole(long id)
        {
            return _dal.QueryByClause(c => c.Id == id);
        }

        public List<Role> GetRoleList(RoleSearch search)
        {
            var where = PredicateBuilder.True<Role>()
                         .WhereIf(true, p => p.IsDel == false)
                         .WhereIf(!string.IsNullOrEmpty(search.Keyword), p => p.RoleName.Contains(search.Keyword));
            return _dal.QueryListByClause(where).ToList();
        }
    }
}
