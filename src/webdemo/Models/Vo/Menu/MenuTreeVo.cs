namespace webdemo.Models.Vo.Menu
{
    public class MenuTreeVo
    {
        public Domain.System.Menu Menu { get; set; }
        public List<MenuTreeVo> Children { get; set; }
    }
}
