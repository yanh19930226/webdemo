using webdemo.Models.Dto.Organization;
using webdemo.Models.Vo.Organization;

namespace webdemo.Services
{
    public interface IOrganizationService
    {
        Organization GetOrganization(long id);

        List<OrganizationVo> GetOrganizationList(OrganizationSearch search);

        List<OrganizationTreeVo> GetOrganizationTree();

        bool Create(Organization organization);

        bool Edit(Organization organization);

        bool Delete(long id);
    }
}
