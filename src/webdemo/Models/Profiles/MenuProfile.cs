using webdemo.Models.Domain.System;

namespace webdemo.Models.Profiles
{
    public class MenuProfile : Profile
    {
        public MenuProfile()
        {
            CreateMap<User, UserListVo>();
            CreateMap<UserCreateDto, User>();
            CreateMap<User, UserEditDto>();
            CreateMap<UserEditDto, User>();
        }
    }
}
