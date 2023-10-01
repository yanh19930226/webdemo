namespace webdemo.Models.Vo.Category
{
    public class CategoryTreeVo
    {
        public Domain.Category Category { get; set; }
        public List<CategoryTreeVo> Children { get; set; }
    }
}
