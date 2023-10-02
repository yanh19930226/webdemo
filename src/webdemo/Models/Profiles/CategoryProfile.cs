using webdemo.Models.Domain.Business;
using webdemo.Models.Dto.Category;
using webdemo.Models.Vo.Category;

namespace webdemo.Models.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryVo>();
            CreateMap<CreateCategoryVo, Category>();
            CreateMap<Category, CreateCategoryVo>();
        }
    }
}
