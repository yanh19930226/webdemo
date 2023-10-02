using webdemo.Models.Vo.Organization;
namespace webdemo.Services.Impl
{
    public class OrganizationService
    {
        private IMapper _mapper;
        private readonly SqlSugarScope _sqlSugarClient;
        private readonly IBaseRepository<Organization> _dal;
        public OrganizationService(IMapper mapper, SqlSugarScope sqlSugarClient, IBaseRepository<Organization> dal)
        {
            _mapper = mapper;
            _sqlSugarClient = sqlSugarClient;
            _dal = dal;
        }

        /// <summary>
        /// 获取子节点
        /// </summary>
        /// <param name="list"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        private List<OrganizationTreeVo> GetChildren(List<Organization> list, long? id)
        {
            List<OrganizationTreeVo> nodeList = new List<OrganizationTreeVo>();
            var children = list.Where(q => q.ParentId == id);
            foreach (var item in children)
            {
                OrganizationTreeVo node = new OrganizationTreeVo();
                node.Organization = item;
                node.Children = GetChildren(list, node.Organization.Id);
                nodeList.Add(node);
            }
            return nodeList;
        }

        /// <summary>
        /// 获取所有子级节点Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private List<long> GetChildIdList(long id)
        {
            var cmdText = $@"
WITH t AS (
		SELECT id, organization_name, parent_id
		FROM organization WITH (NOLOCK)
		WHERE id = @Id
		UNION ALL
		SELECT organization.id, organization.organization_name, organization.parent_id
		FROM organization, t
		WHERE organization.parent_id = t.id
	)
SELECT *
FROM t";
            var categoryIdList = _sqlSugarClient.Ado.SqlQuery<long>(cmdText, new
            {
                Id = id,
            });
            return categoryIdList;
        }

        /// <summary>
        /// 删除Organization
        /// </summary>
        /// <param name="categoryIdList"></param>
        /// <returns></returns>
        private int Delete(List<long> idList)
        {
            if (idList == null || idList.Count == 0)
            {
                return 0;
            }

            var cmdText = $@"
            DELETE FROM Organization
            WHERE id IN ({string.Join(",", idList)})";
            return _sqlSugarClient.Ado.ExecuteCommand(cmdText);
        }

        public Organization GetOrganization(long id)
        {
            return _dal.QueryByClause(c => c.Id == id);
        }

        public List<OrganizationVo> GetOrganizationList()
        {
            var queryText = string.Empty;
            var cmdText = $@"
WITH RECURSIVE cte_child (organization_type, id, organization_name, parent_id,leader_id, sort, remark, level) AS (
		SELECT organization_type, id, organization_name, parent_id,leader_id, sort, remark, 1 as level
		FROM organization
		WHERE parent_id = 0
		UNION ALL
		SELECT a.organization_type, a.id, a.organization_name, a.parent_id, a.sort,a.leader_id
			, a.remark, b.level + 1
		FROM organization a
			INNER JOIN cte_child b ON a.parent_id = b.id
	)
SELECT *
FROM cte_child
WHERE 1=1
{queryText}
ORDER BY sort DESC, id ASC";

            var result = _sqlSugarClient.Ado.SqlQuery<OrganizationVo>(cmdText, new
            {
            }).ToList();
            return result;
        }

        public List<OrganizationTreeVo> GetOrganizationTree()
        {
            var result = _dal.QueryListByClause(p => p.IsDel == false).ToList();
            return GetChildren(result, 0);
        }

        public DemoResult Create(Organization organization)
        {
            DemoResult result = new DemoResult();
            if (_dal.Insert(organization) > 0)
            {
                result.Success();
            }
            else
            {
                result.Failed();
            }
            return result;
        }

        public DemoResult Edit(Organization organization)
        {
            DemoResult result = new DemoResult();
            if (_dal.Update(organization))
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
            var idList = GetChildIdList(id);
            var intRes = Delete(idList);
            if (intRes > 0)
            {
                result.Success();
            }
            else
            {
                result.Failed();
            }
            return result;
        }
    }
}
