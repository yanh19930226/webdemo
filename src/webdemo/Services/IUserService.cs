using webdemo.Models.Vo;
using Webdiyer.AspNetCore;

namespace webdemo.Services
{
    public interface IUserService
    {
        //PageResult<UserListVo> GetPageResult(UserSearch search);

        IPagedList<UserListVo> GetPageResult(UserSearch search);
    }
}
