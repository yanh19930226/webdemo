namespace webdemo.Services
{
    public interface IUserService
    {
        IPagedList<UserListVo> GetPageResult(UserSearch search);

        IPagedList<Order> GetPagedOrders(int pageIndex, int pageSize, string companyName = null);
    }
}
