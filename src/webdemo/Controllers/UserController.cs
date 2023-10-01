namespace webdemo.Controllers
{
    public class UserController : Controller
    {
        private readonly IBaseRepository<User> _dal;
        private IMapper _mapper;
        private IUserService _userService;
        public UserController(IBaseRepository<User> dal, IMapper mapper, IUserService userService)
        {
            _dal = dal;
            _mapper = mapper;
            _userService= userService;
        }

        public IActionResult Index(UserSearch userSearch)
        {
            userSearch.StartDate = DateTime.Now.AddDays(-7);
            userSearch.EndDate = DateTime.Now;
            return View(userSearch);
        }
        public IActionResult DoList(UserSearch userSearch) 
        {
            var result = _userService.GetPageResult(userSearch);
            return PartialView(result);
        }

        public IActionResult Add()
        {
            UserCreateDto userCreateDto = new UserCreateDto();
            return View(userCreateDto);
        }
        [HttpPost]
        public IActionResult DoAdd(UserCreateDto dto)
        {
            DemoResult result = new DemoResult();
            if (_dal.Insert(_mapper.Map<User>(dto)) > 0)
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
            var edit = _dal.QueryByClause(p => p.Id == Id);
            return View(_mapper.Map<UserEditDto>(edit));
        }
        [HttpPost]
        public IActionResult DoEdit(UserEditDto dto)
        {
            DemoResult result = new DemoResult();
            if (_dal.Update(_mapper.Map<User>(dto)))
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
            var delete = _dal.QueryByClause(p => p.Id == Id);
            delete.IsDel = true;
            if (_dal.Update(delete))
            {
                result.Success("删除成功");
            }
            return Ok(result);
        }
    }
}
