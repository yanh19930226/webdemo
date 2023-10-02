using webdemo.Models.Dto.Organization;

namespace webdemo.Controllers.System
{
    public class OrganizationController : Controller
    {
        private IMapper _mapper;
        private IOrganizationService _organizationService;
        public OrganizationController(IMapper mapper, IOrganizationService organizationService)
        {
            _mapper = mapper;
            _organizationService = organizationService;
        }

        public IActionResult Index(OrganizationSearch search)
        {
            return View(search);
        }

        public IActionResult GetOrganizationList(OrganizationSearch search)
        {
            var result = _organizationService.GetOrganizationList(search);
            return Json(result);
        }

        public IActionResult Create()
        {
            Organization organization = new Organization();
            return PartialView(organization);
        }

        [HttpPost]
        public IActionResult DoCreate(Organization dto)
        {
            return Ok(_organizationService.Create(dto));
        }

        public IActionResult Edit(long id)
        {
            var edit = _organizationService.GetOrganization(id);
            return View(edit);
        }
        [HttpPost]
        public IActionResult DoEdit(Organization dto)
        {
            return Ok(_organizationService.Edit(dto));
        }

        public IActionResult Delete(long id)
        {
            return Ok(_organizationService.Delete(id));
        }
    }
}
