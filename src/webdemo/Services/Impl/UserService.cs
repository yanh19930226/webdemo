using PagedList.Core;
using webdemo.Models.Vo;

namespace webdemo.Services.Impl
{
    public class UserService : IUserService
    {
        private DemoDbContext _dbContext;
        private IMapper _mapper;
        public UserService(DemoDbContext dbContext, IMapper mapper)
        {
            _dbContext= dbContext;
            _mapper= mapper;
        }

        public PageResult<UserListVo> GetPageResult(UserSearch search)
        {
            PageResult<UserListVo> pageResult = new PageResult<UserListVo>();
            
            var where = PredicateBuilder.True<User>()
                         .WhereIf(true, p => p.IsDel == false)
                         .WhereIf(!string.IsNullOrEmpty(search.Keyword), p => p.UserName.Contains(search.Keyword));

            var total = _dbContext.User.Where(where).Count();
            var result = _dbContext.User.Where(where).Skip((search.Page - 1) * search.PageSize).Take(search.PageSize).Select(p=>_mapper.Map<UserListVo>(p));

            pageResult.FilterData = search;
            pageResult.Data = new StaticPagedList<UserListVo>(result, search.Page, search.PageSize, total);

            return pageResult;
        }
    }
}
