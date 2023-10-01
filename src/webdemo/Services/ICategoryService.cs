using Microsoft.EntityFrameworkCore;
using webdemo.Models.Dto.Category;
using webdemo.Models.Vo.Category;

namespace webdemo.Services
{
    public interface ICategoryService
    {

        Category GetCategory(long categoryId);
       
        List<CategoryVo> GetCategoryList(int serviceId);

        List<CategoryTreeVo> GetCategoryTree(int serviceId);

        DemoResult CreateCategory(CreateCategoryVo createCategoryVo);
        
        DemoResult EditCategory(CreateCategoryVo createCategoryVo);

        DemoResult DeleteCategory(int categoryId);
    }
}
