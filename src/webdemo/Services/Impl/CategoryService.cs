using webdemo.Models.Dto.Category;
using webdemo.Models.Vo.Category;

namespace webdemo.Services.Impl
{
    public class CategoryService : ICategoryService
    {
        private IMapper _mapper;
        private DemoDbContext _dbContext;
        public CategoryService(IMapper mapper, DemoDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
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
		SELECT Id, CategoryName, ParentId
		FROM [Category] WITH (NOLOCK)
		WHERE Id = @CategoryId
		UNION ALL
		SELECT [Category].Id, [Category].CategoryName, [Category].ParentId
		FROM [Category], t
		WHERE [Category].ParentId = t.Id
	)
SELECT *
FROM t
            ";
            var categoryIdList = _dbContext.Category.FromSqlRaw<Category>(cmdText, new {
                CategoryId = categoryId,
            }).Select(p=>p.Id).ToList();
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
            UPDATE [Article]
            SET [Status] = 0
            WHERE CategoryId IN ({string.Join(",", categoryIdList)})";
            _dbContext.Database.ExecuteSqlRaw(cmdText);

            cmdText = $@"
            DELETE FROM [Category]
            WHERE CategoryId IN ({string.Join(",", categoryIdList)})";

            return _dbContext.Database.ExecuteSqlRaw(cmdText);
        }

        public Category GetCategory(long categoryId)
        {
            return _dbContext.Category.FirstOrDefault(c => c.Id == categoryId);
        }

        public List<CategoryVo> GetCategoryList(int serviceId)
        {
            var cmdText = $@"
WITH RECURSIVE CTE_CHILD (Id, ServiceId, CategoryName, ParentId, Sort, STATUS, LEVEL) AS (
		SELECT ServiceId, Id, CategoryName, ParentId, Sort, STATUS, 1 AS LEVEL
		FROM Category
		WHERE ParentId = 0
		UNION ALL
		SELECT A.Id, A.ServiceId, A.CategoryName, A.ParentId, A.Sort
			, A.Status, B.Level + 1
		FROM Category A
			INNER JOIN CTE_CHILD B ON A.ParentId = B.Id
	)
SELECT *
FROM CTE_CHILD
WHERE ServiceId = @ServiceId
	AND STATUS = 1
ORDER BY Sort DESC, Id ASC
            ";

            var result = _dbContext.CategoryVo.FromSqlRaw<CategoryVo>(cmdText, new
            {
                ServiceId = serviceId
            }).ToList();
            return result;
        }

        public List<CategoryTreeVo> GetCategoryTree(int serviceId)
        {
            var result = _dbContext.Category.Where(p => p.ServiceId == serviceId && p.Status == 0).ToList();
            return GetChildren(result, 0);
        }

        public DemoResult CreateCategory(CreateCategoryVo createCategoryVo)
        {
            DemoResult result = new DemoResult();
            Category category = new Category();
            _mapper.Map<CreateCategoryVo>(category);
            category.CreateTime = DateTime.Now;
            _dbContext.Add(category);
            if (_dbContext.SaveChanges() > 0)
            {
                result.Success("修改成功");
            }
            else
            {
                result.Failed("修改失败");
            }
            return result;
        }

        public DemoResult EditCategory(CreateCategoryVo createCategoryVo)
        {
            DemoResult result = new DemoResult();
            var category = _dbContext.Category.FirstOrDefault(c => c.Id == createCategoryVo.CategoryId);
            _mapper.Map<CreateCategoryVo>(category);
            _dbContext.Update(category);
            if (_dbContext.SaveChanges() > 0)
            {
                result.Success("修改成功");
            }
            else
            {
                result.Failed("修改失败");
            }
            return result;
        }

        public DemoResult DeleteCategory(int categoryId)
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
