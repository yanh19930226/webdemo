using webdemo.Models.Dto.Category;

namespace webdemo.Controllers
{
    public class CategoryController : Controller
    {
        private IMapper _mapper;
        private ICategoryService _categoryService;
        public CategoryController(IMapper mapper, ICategoryService categoryService)
        {
            _mapper = mapper;
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetCategoryList(int serviceId)
        {
            var result = _categoryService.GetCategoryList(serviceId);
            return Json(result);
        }

        public IActionResult Add(int serviceId, int parentId)
        {
            CreateCategoryVo createCategoryVo = new CreateCategoryVo();
            createCategoryVo.ServiceId = serviceId;
            createCategoryVo.ParentId = parentId;
            return PartialView(createCategoryVo);
        }
       
        [HttpPost]
        public IActionResult DoAdd(CreateCategoryVo dto)
        {
            return Ok(_categoryService.CreateCategory(dto));
        }

        public IActionResult Edit(long id)
        {
            var edit = _categoryService.GetCategory(id);
            return View(_mapper.Map<UserEditDto>(edit));
        }
        [HttpPost]
        public IActionResult DoEdit(CreateCategoryVo dto)
        {
            return Ok(_categoryService.EditCategory(dto));
        }

        public IActionResult Delete(long id)
        {
            return Ok(_categoryService.DeleteCategory(id));
        }
    }
}
