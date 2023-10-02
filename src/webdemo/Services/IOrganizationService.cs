using webdemo.Models.Dto.Organization;
using webdemo.Models.Vo.Organization;

namespace webdemo.Services
{
    public interface IOrganizationService
    {
        Organization GetOrganization(long id);

        List<OrganizationVo> GetOrganizationList(OrganizationSearch search);

        List<OrganizationTreeVo> GetOrganizationTree();

        DemoResult Create(Organization organization);

        DemoResult Edit(Organization organization);

        DemoResult Delete(long id);
    }
}
