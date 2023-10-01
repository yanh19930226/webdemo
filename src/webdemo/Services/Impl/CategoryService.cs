using SqlSugar;
using webdemo.Models.Dto.Category;
using webdemo.Models.Vo.Category;

namespace webdemo.Services.Impl
{
    public class CategoryService : ICategoryService
    {
        private IMapper _mapper;
        private readonly SqlSugarScope _sqlSugarClient;
        private readonly IBaseRepository<Category> _dal;
        public CategoryService(IMapper mapper, SqlSugarScope sqlSugarClient, IBaseRepository<Category> dal)
        {
            _mapper = mapper;
            _sqlSugarClient=sqlSugarClient;
            _dal = dal;
        }
        /// <summary>
        /// 获取子节点
        /// </summary>
        /// <param name="list"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        private List<CategoryTreeVo> GetChildren(List<Category> list, long? categoryId)
        {
            List<CategoryTreeVo> nodeList = new List<CategoryTreeVo>();
            var children = list.Where(q => q.ParentId == categoryId);
            foreach (var item in children)
            {
                CategoryTreeVo node = new CategoryTreeVo();
                node.Category = item;
                node.Children = GetChildren(list, node.Category.Id);
                nodeList.Add(node);
            }
            return nodeList;
        }

        /// <summary>
        /// 获取所有子级节点Id
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        private List<long> GetChildIdList(long categoryId)
        {
            var cmdText = $@"
WITH t AS (
		SELECT Id, category_name, parent_id
		FROM category WITH (NOLOCK)
		WHERE Id = @CategoryId
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
                CategoryId = categoryId,
            });
            return categoryIdList;
        }

        /// <summary>
        /// 删除Category
        /// </summary>
        /// <param name="categoryIdList"></param>
        /// <returns></returns>
        private int DeleteCategory(List<long> categoryIdList)
        {
            if (categoryIdList == null || categoryIdList.Count == 0)
            {
                return 0;
            }

            var cmdText = $@"
            UPDATE article
            SET [Status] = 0
            WHERE category_id IN ({string.Join(",", categoryIdList)})";
            _sqlSugarClient.Ado.ExecuteCommand(cmdText);

            cmdText = $@"
            DELETE FROM category
            WHERE id IN ({string.Join(",", categoryIdList)})";
            return _sqlSugarClient.Ado.ExecuteCommand(cmdText);
        }

        public Category GetCategory(long categoryId)
        {
            return _dal.QueryByClause(c => c.Id == categoryId);
        }

        public List<CategoryVo> GetCategoryList(int serviceId)
        {
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
WHERE service_id = @ServiceId
	AND status = 0
ORDER BY sort DESC, id ASC
            ";

            var result = _sqlSugarClient.Ado.SqlQuery<CategoryVo>(cmdText, new
            {
                ServiceId = serviceId
            }).ToList();
            return result;
        }

        public List<CategoryTreeVo> GetCategoryTree(int serviceId)
        {
            var result = _dal.QueryListByClause(p => p.ServiceId == serviceId && p.Status == 0).ToList();
            return GetChildren(result, 0);
        }

        public DemoResult CreateCategory(CreateCategoryVo createCategoryVo)
        {
            DemoResult result = new DemoResult();
            Category category = _mapper.Map<Category>(createCategoryVo);
            if (_dal.Insert(category) > 0)
            {
                result.Success("添加成功");
            }
            else
            {
                result.Failed("添加失败");
            }
            return result;
        }

        public DemoResult EditCategory(CreateCategoryVo createCategoryVo)
        {
            DemoResult result = new DemoResult();
            var category=_mapper.Map<Category>(createCategoryVo);
            if (_dal.Update(category))
            {
                result.Success("修改成功");
            }
            else
            {
                result.Failed("修改失败");
            }
            return result;
        }

        public DemoResult DeleteCategory(long categoryId)
        {
            DemoResult result = new DemoResult();
            var categoryIdList = GetChildIdList(categoryId);
            var intRes = DeleteCategory(categoryIdList);
            if (intRes > 0)
            {
                result.Success("删除成功");
            }
            else
            {
                result.Failed("删除失败");
            }
            return result;
        }
    }
}
