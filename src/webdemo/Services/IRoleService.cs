using webdemo.Models.Dto.Role;

namespace webdemo.Services
{
    public interface IRoleService
    {
        Role GetRole(long id);

        List<Role> GetRoleList(RoleSearch search);

        DemoResult Create(Role menu);

        DemoResult Edit(Role menu);

        DemoResult Delete(long id);
    }
}
