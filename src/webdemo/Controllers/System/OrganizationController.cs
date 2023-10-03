using webdemo.Models.Dto.Organization;

namespace webdemo.Controllers.System
{
    public class OrganizationController : Controller
    {
        private IMapper _mapper;
        private IOrganizationService _orgService;
        public OrganizationController(IMapper mapper, IOrganizationService orgService)
        {
            _mapper = mapper;
            _orgService = orgService;
        }

        public IActionResult Index(OrganizationSearch search)
        {
            return View(search);
        }

        public IActionResult GetOrganizationList(OrganizationSearch search)
        {
            var result = _orgService.GetOrganizationList(search);
            return Json(result);
        }

        public IActionResult Create()
        {
            Organization org = new Organization();
            return PartialView(org);
        }

        [HttpPost]
        public IActionResult DoCreate(Organization dto)
        {

            DemoResult result = new DemoResult();
            if (_orgService.Create(dto))
            {
                result.Success();
            }
            else
            {
                result.Failed();
            }
            return Ok(result);
        }

        public IActionResult Edit(long id)
        {
            var edit = _orgService.GetOrganization(id);
            return View(edit);
        }
        [HttpPost]
        public IActionResult DoEdit(Organization dto)
        {
            DemoResult result = new DemoResult();
            if (_orgService.Edit(dto))
            {
                result.Success();
            }
            else
            {
                result.Failed();
            }
            return Ok(result);
        }

        public IActionResult Delete(long id)
        {
            DemoResult result = new DemoResult();
            if (_orgService.Delete(id))
            {
                result.Success();
            }
            else
            {
                result.Failed();
            }
            return Ok(result);
        }
    }
}
