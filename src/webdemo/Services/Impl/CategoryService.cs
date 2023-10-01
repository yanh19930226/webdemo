//namespace webdemo.Services.Impl
//{
//    public class CategoryService: ICategoryService
//    {
//        private IMapper _mapper;
//        private DemoDbContext _dbContext;
//        public CategoryService(IMapper mapper, DemoDbContext dbContext)
//        {
//            _mapper = mapper;
//            _dbContext = dbContext;
//        }
//        private List<HelpCategoryTreeViewModel> GetChildren(List<HelpCategory> list, int? categoryId)
//        {
//            List<HelpCategoryTreeViewModel> nodeList = new List<HelpCategoryTreeViewModel>();
//            var children = list.Where(q => q.ParentId == categoryId);
//            foreach (var item in children)
//            {
//                HelpCategoryTreeViewModel node = new HelpCategoryTreeViewModel();
//                node.HelpCategory = item;
//                node.Children = GetChildren(list, node.HelpCategory.CategoryId);
//                nodeList.Add(node);
//            }
//            return nodeList;
//        }

//        /// <summary>
//        /// 获取所有子级节点
//        /// </summary>
//        /// <param name="categoryId"></param>
//        /// <returns></returns>
//        private List<int> GetChildIdList(int categoryId)
//        {
//            var cmdText = $@"
//WITH t AS (
//		SELECT CategoryId, CategoryName, ParentId
//		FROM [HelpCategory] WITH (NOLOCK)
//		WHERE CategoryId = @CategoryId
//		UNION ALL
//		SELECT [HelpCategory].CategoryId, [HelpCategory].CategoryName, [HelpCategory].ParentId
//		FROM [HelpCategory], t
//		WHERE [HelpCategory].ParentId = t.CategoryId
//	)
//SELECT CategoryId
//FROM t
//            ";

//            var categoryIdList = _repository.Change<HelpCategory>().GetList<int>(cmdText, new
//            {
//                CategoryId = categoryId,
//            }).ToList();

//            return categoryIdList;
//        }
        

//        public List<HelpCategoryViewModel> GetHelpCategoryList(int serviceId)
//        {
//            var cmdText = $@"
//WITH CTE_CHILD (ServiceId, CategoryId, CategoryName, ParentId, Sort, [Status], [Level]) AS (
//		SELECT ServiceId, CategoryId, CategoryName, ParentId, Sort
//			, [Status], 1 AS [Level]
//		FROM [HelpCategory]
//		WHERE ParentId = 0
//		UNION ALL
//		SELECT A.ServiceId, A.CategoryId, A.CategoryName, A.ParentId, A.Sort
//			, A.[Status], B.[Level] + 1
//		FROM [HelpCategory] A
//			INNER JOIN CTE_CHILD B ON A.ParentId = B.CategoryId
//	)
//SELECT *
//FROM CTE_CHILD
//WHERE ServiceId = @ServiceId
//	AND [Status] = 1
//ORDER BY Sort DESC, CategoryId ASC
//            ";

//            var result = _repository.Change<HelpCategory>().GetList<HelpCategoryViewModel>(cmdText, new
//            {
//                ServiceId = serviceId
//            }).ToList();
//            return result;
//        }
//        public List<HelpCategoryTreeViewModel> GetHelpCategoryTree(int serviceId)
//        {
//            var result = _helpCategoryRepository.GetHelpCategoryList(new HelpCategorySearchFilter
//            {
//                ServiceId = serviceId,
//                Status = 0
//            }, HelpCategorySearchSort.CategoryIdDesc, int.MaxValue);
//            return GetChildren(result, 0);
//        }
//        public HelpCategory GetHelpCategory(int categoryId)
//        {
//            return _helpCategoryRepository.GetHelpCategory(categoryId);
//        }
//        public ApiResult CreateCategory(CreateHelpCategoryViewModel postModel)
//        {
//            HelpCategory helpCategory = new HelpCategory();
//            postModel.Map(helpCategory);
//            helpCategory.CreateTime = DateTime.Now;
//            var result = _helpCategoryRepository.AddHelpCategory(helpCategory);
//            if (result)
//            {
//                return ApiResult.Success("添加成功");
//            }
//            return ApiResult.Error("添加失败");
//        }
//        public ApiResult EditCategory(CreateHelpCategoryViewModel postModel)
//        {
//            var helpCategory = _helpCategoryRepository.GetHelpCategory(postModel.CategoryId);
//            postModel.Map(helpCategory);
//            var result = _helpCategoryRepository.UpdateHelpCategory(helpCategory);
//            if (result)
//            {
//                return ApiResult.Success("修改成功");
//            }
//            return ApiResult.Error("修改失败");
//        }
//        public ApiResult DeleteCategory(int categoryId)
//        {
//            var categoryIdList = GetChildIdList(categoryId);
//            var result = _helpCategoryRepository.DeleteHelpCategory(categoryIdList);
//            if (result > 0)
//            {
//                return ApiResult.Success("删除成功");
//            }
//            return ApiResult.Error("删除失败");
//        }
//    }
//}
