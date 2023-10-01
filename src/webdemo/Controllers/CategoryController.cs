namespace webdemo.Controllers
{
    public class CategoryController : Controller
    {
        private DemoDbContext _dbContext;
        private IMapper _mapper;
        private ICategoryService _categoryService;
        public CategoryController(DemoDbContext dbContext, IMapper mapper, ICategoryService categoryService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            return View();
        }
        

        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DoAdd(UserCreateDto dto)
        {
            DemoResult result = new DemoResult();
            _dbContext.User.Add(_mapper.Map<User>(dto));
            if (_dbContext.SaveChanges() > 0)
            {
                result.Success("添加成功");
            }
            else
            {
                result.Failed("添加失败");
            }
            return Ok(result);
        }

        public IActionResult Edit(int Id)
        {
            var edit = _dbContext.User.Where(p => p.Id == Id).FirstOrDefault();
            return View(_mapper.Map<UserEditDto>(edit));
        }
        [HttpPost]
        public IActionResult DoEdit(UserEditDto dto)
        {
            DemoResult result = new DemoResult();

            var edit = _dbContext.User.Where(p => p.Id == dto.Id).AsNoTracking().FirstOrDefault();

            _dbContext.User.Update(_mapper.Map<User>(dto));

            if (_dbContext.SaveChanges() > 0)
            {
                result.Success("修改成功");
            }
            else
            {
                result.Failed("修改失败");
            }

            return Ok(result);
        }

        public IActionResult Delete(int Id)
        {
            DemoResult result = new DemoResult();
            var delete = _dbContext.User.Where(p => p.Id == Id).FirstOrDefault();
            delete.IsDel = true;
            _dbContext.User.Update(delete);
            if (_dbContext.SaveChanges() > 0)
            {
                result.Success("删除成功");
            }
            return Ok(result);
        }
    }
}
