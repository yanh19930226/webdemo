namespace webdemo.Models.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserListVo>();
            CreateMap<UserCreateDto, User>();
            CreateMap<User, UserEditDto>();
            CreateMap<UserEditDto, User>();
        }
    }
}
