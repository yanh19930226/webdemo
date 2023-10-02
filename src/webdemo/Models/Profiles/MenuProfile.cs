using webdemo.Models.Dto.Menu;

namespace webdemo.Models.Profiles
{
    public class MenuProfile : Profile
    {
        public MenuProfile()
        {
            CreateMap<Menu, MenuEditDto>();
            CreateMap<MenuEditDto, Menu>();
        }
    }
}
