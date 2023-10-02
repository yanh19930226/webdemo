namespace webdemo.Models.Vo.Organization
{
    public class OrganizationTreeVo
    {
        public Domain.System.Organization Organization { get; set; }
        public List<OrganizationTreeVo> Children { get; set; }
    }
}
