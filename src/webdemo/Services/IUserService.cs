using webdemo.Models.Vo;

namespace webdemo.Services
{
    public interface IUserService
    {
        PageResult<UserListVo> GetPageResult(UserSearch search);
    }
}
