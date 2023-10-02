namespace webdemo.Models.Vo.Category
{
    public class CategoryTreeVo
    {
        public Domain.Business.Category Category { get; set; }
        public List<CategoryTreeVo> Children { get; set; }
    }
}
