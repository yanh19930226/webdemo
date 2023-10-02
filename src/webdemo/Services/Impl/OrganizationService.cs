using webdemo.Models.Dto.Category;
using webdemo.Models.Vo.Category;
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
		SELECT Id, category_name, parent_id
		FROM category WITH (NOLOCK)
		WHERE Id = @Id
		UNION ALL
		SELECT category.Id, category.category_name, category.parent_id
		FROM category, t
		WHERE category.parent_id = t.Id
	)
SELECT *
FROM t
            ";
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

        public List<CategoryVo> GetOrganizationList()
        {
            var queryText = string.Empty;
            var cmdText = $@"
WITH RECURSIVE cte_child (service_id, id, category_name, parent_id, sort, status, LEVEL) AS (
		SELECT service_id, id, category_name, parent_id, sort, status, 1 AS LEVEL
		FROM category
		WHERE parent_id = 0
		UNION ALL
		SELECT a.service_id, a.id, a.category_name, a.parent_id, a.sort
			, a.status, b.level + 1
		FROM category a
			INNER JOIN cte_child b ON a.parent_id = b.id
	)
SELECT *
FROM cte_child
WHERE 1=1
{queryText}
	AND status = 0
ORDER BY sort DESC, id ASC";

            var result = _sqlSugarClient.Ado.SqlQuery<CategoryVo>(cmdText, new
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
