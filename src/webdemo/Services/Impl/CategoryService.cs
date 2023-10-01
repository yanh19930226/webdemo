using NuGet.Protocol.Core.Types;
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
        public int DeleteCategory(List<long> categoryIdList)
        {
            //            if (categoryIdList == null || categoryIdList.Count == 0)
            //            {
            //                return 0;
            //            }

            //            var parameters = new
            //            {
            //                CategoryIds = string.Join(",", categoryIdList)
            //            };

            //            var cmdText = $@"
            //UPDATE [HelpArticle]
            //SET [Status] = 0
            //WHERE CategoryId IN (
            //		SELECT *
            //		FROM SplitInt32Table(@CategoryIds, ','))";
            //            _repository.Execute(cmdText, parameters);

            //            cmdText = $@"
            //DELETE FROM [HelpCategory]
            //WHERE CategoryId IN (
            //		SELECT *
            //		FROM SplitInt32Table(@CategoryIds, ','))
            //            ";

            //            return _repository.Execute(cmdText, parameters);


            return 1;
        }

        public Category GetCategory(long categoryId)
        {
            return _dbContext.Category.FirstOrDefault(c => c.Id == categoryId);
        }

        public List<CategoryVo> GetCategoryList(int serviceId)
        {
            var cmdText = $@"
WITH CTE_CHILD (ServiceId, Id, CategoryName, ParentId, Sort, [Status], [Level]) AS (
		SELECT ServiceId, Id, CategoryName, ParentId, Sort, [Status], 1 AS [Level]
		FROM [Category]
		WHERE ParentId = 0
		UNION ALL
		SELECT A.ServiceId, A.Id, A.CategoryName, A.ParentId, A.Sort
			, A.[Status], B.[Level] + 1
		FROM [Category] A
			INNER JOIN CTE_CHILD B ON A.ParentId = B.Id
	)
SELECT *
FROM CTE_CHILD
WHERE ServiceId = @ServiceId
	AND [Status] = 1
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
            DeleteCategory(categoryIdList);
            return result;
        }
    }
}
