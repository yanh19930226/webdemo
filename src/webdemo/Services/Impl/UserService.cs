using SqlSugar;
using System.Linq;
using webdemo.Models.Domain.Business;
using webdemo.Models.Domain.System;

namespace webdemo.Services.Impl
{
    public class UserService : IUserService
    {
        private IMapper _mapper;
        private readonly IBaseRepository<User> _dal;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment Env;
        public UserService(IMapper mapper, IBaseRepository<User> dal, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            _mapper = mapper;
            _dal = dal;
            Env = env;
        }

        public IPagedList<UserListVo> GetPageResult(UserSearch search)
        {
            var where = PredicateBuilder.True<User>()
                         .WhereIf(true, p => p.IsDel == false)
                         .WhereIf(!string.IsNullOrEmpty(search.Keyword), p => p.UserName.Contains(search.Keyword));

            var result = _dal.QueryListByClause(where).Select(p => _mapper.Map<UserListVo>(p));

            return result.ToPagedList(search.PageIndex, search.PageSize);
        }


        public IPagedList<Order> GetPagedOrders(int pageIndex, int pageSize, string companyName = null)
        {
            var path = Path.Combine(Env.WebRootPath, "orders.json");
            var ods = Newtonsoft.Json.JsonConvert.DeserializeObject<Order[]>(System.IO.File.ReadAllText(path));
            if (!string.IsNullOrWhiteSpace(companyName))
            {
                return ods.Where(o => o.CompanyName.Contains(companyName)).OrderBy(o => o.OrderId).ToPagedList(pageIndex, pageSize);
            }
            return ods.OrderBy(o => o.OrderId).ToPagedList(pageIndex, pageSize);
        }
    }
}
