using webdemo.Services;

namespace webdemo.Controllers
{
    public class UserController : Controller
    {
        private DemoDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public UserController(DemoDbContext dbContext, IMapper mapper, IUserService userService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userService= userService;
        }
        public IActionResult Index(UserSearch userSearch)
        {
            return View(userSearch);
        }
        public IActionResult DoList(UserSearch userSearch)
        {
            var where = PredicateBuilder.True<User>();
            where = where.WhereIf(true,p=>p.IsDel==false)
                         .WhereIf(!string.IsNullOrEmpty(userSearch.Keyword),p => p.UserName.Contains(userSearch.Keyword));
            var result = _userService.GetPageResult(userSearch);
          
            return PartialView(result);
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
        public ActionResult Edit(int Id)
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
