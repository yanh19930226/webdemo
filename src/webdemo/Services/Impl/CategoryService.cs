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
        /// 获取所有子级节点
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
        public Category GetHelpCategory(int categoryId)
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

            var result = _dbContext.Category.FromSqlRaw<Category>(cmdText, new
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

        //public ApiResult CreateCategory(CreateHelpCategoryViewModel postModel)
        //{
        //    HelpCategory helpCategory = new HelpCategory();
        //    postModel.Map(helpCategory);
        //    helpCategory.CreateTime = DateTime.Now;
        //    var result = _helpCategoryRepository.AddHelpCategory(helpCategory);
        //    if (result)
        //    {
        //        return ApiResult.Success("添加成功");
        //    }
        //    return ApiResult.Error("添加失败");
        //}
        //public ApiResult EditCategory(CreateHelpCategoryViewModel postModel)
        //{
        //    var helpCategory = _helpCategoryRepository.GetHelpCategory(postModel.CategoryId);
        //    postModel.Map(helpCategory);
        //    var result = _helpCategoryRepository.UpdateHelpCategory(helpCategory);
        //    if (result)
        //    {
        //        return ApiResult.Success("修改成功");
        //    }
        //    return ApiResult.Error("修改失败");
        //}
        //public ApiResult DeleteCategory(int categoryId)
        //{
        //    var categoryIdList = GetChildIdList(categoryId);
        //    var result = _helpCategoryRepository.DeleteHelpCategory(categoryIdList);
        //    if (result > 0)
        //    {
        //        return ApiResult.Success("删除成功");
        //    }
        //    return ApiResult.Error("删除失败");
        //}
    }
}
